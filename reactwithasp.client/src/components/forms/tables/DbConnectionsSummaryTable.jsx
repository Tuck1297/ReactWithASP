import React, { useState } from "react";
import Modal from "../../Modal";
import SmallSpinner from "../../loading/SmallSpinner";
import { dbCSService } from "../../../services/dbCSService";
import { alertService } from "../../../services/alertService";

const Table = ({ data, switchView }) => {
  const [tableData, setTableData] = useState(data);
  const [modalOpen, setModalOpen] = useState(false);
  const [toDelete, setToDelete] = useState(null);
  const [deleting, setDeleting] = useState(null);

  // TODOs will be sections that will connect with api connection of database.

  const accessDatabaseContents = (index) => {
    // TODO - Connect database update action here
    // console.log("Access Database...", tableData[index]);
    switchView(tableData[index]);
  };

  const handleDelete = async (index) => {
    // setInterval( async () => {
      await dbCSService
      .delete(tableData[index].id)
      .then((result) => {
        alertService.success(
          `Information associated with ${tableData[index].dbName} has been deleted.`
        );
        const updatedData = [...tableData];
        updatedData.splice(index, 1);
        setTableData(updatedData);
        setDeleting(null);
      })
      .catch((error) => {
        alertService.error(
          "Cannot delete connection information at this time."
        );
        setDeleting(null);
      });
    // }, 5000);
  };

  const numberOfNeededCols = Object.keys(data[0]).length + 1;
  let percentage = 100 / numberOfNeededCols;
  if (numberOfNeededCols > 8) {
    percentage = 0;
  }

  return (
    <>
      <Modal
        message="Are you sure you want to delete this? After this point changes cannot be reverted!"
        btnActionName="Delete"
        setModalOpen={setModalOpen}
        modalOpen={modalOpen}
        btnAction={() => {
          handleDelete(toDelete);
          setModalOpen(false);
          setToDelete(null);
          setDeleting(toDelete);
        }}
      ></Modal>
      <div className="d-flex justify-content-center flex-column">
        <div className="table-container">
          <table className="data-table w-100" style={{ minWidth: "500px" }}>
            <thead>
              <tr>
                {Object.keys(data[0]).map((key) => (
                  <th
                    key={key}
                    className="text-center pt-3 pb-3"
                    style={{
                      textTransform: "capitalize",
                      width: `${percentage}%`,
                      minWidth: "100px",
                    }}
                  >
                    {key}
                  </th>
                ))}
                <th className="text-center pt-3 pb-3">
                  Database Connection Actions
                </th>
              </tr>
            </thead>
            <tbody>
              {tableData.map((row, index) => (
                <tr key={index}>
                  {Object.keys(row).map((key) => (
                    <td key={key} className="text-center pt-3 pb-3">
                      {row[key]}
                    </td>
                  ))}
                  <td className="d-flex justify-content-center align-items-center">
                    <button
                      className="btn btn-primary m-1"
                      onClick={() => accessDatabaseContents(index)}
                    >
                      Access
                    </button>
                    <button
                      className="btn btn-primary m-1"
                      onClick={() => {
                        setModalOpen(true);
                        setToDelete(index);
                      }}
                      disabled={deleting === index}
                    >
                      {deleting === index ? <SmallSpinner /> : "Delete"}
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          <h4 className="text-center w-100">
            {tableData.length === 0 ? "Nothing to see here..." : ""}
          </h4>
        </div>
      </div>
    </>
  );
};

export default Table;
