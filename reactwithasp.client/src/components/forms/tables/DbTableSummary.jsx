import React, { useState } from "react";
import Modal from "../../Modal";
import { alertService } from "../../../services/alertService";
import { externalDbService } from "../../../services/externalDbService";

const Table = ({ data, switchView, currentDBInteracting }) => {
  const [tableData, setTableData] = useState(data);
  const [modalOpen, setModalOpen] = useState(false);
  const [toDelete, setToDelete] = useState(null);

  // need to take a look at table rows index (showing -1 when there are one or two rows in the table)

  const handleDelete = (index) => {
    console.log(index);
    setTimeout(async () => {
      await externalDbService
        .deleteTable(currentDBInteracting, tableData[index].tableName)
        .then((result) => {
          alertService.success("Table has successfully been deleted");
          console.log(result);
          const updatedData = [...tableData];
          updatedData.splice(index, 1);
          setTableData(updatedData);
        })
        .catch((error) => {
          alertService.error(error);
        });
    }, 1000);
  };

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
        }}
      ></Modal>
      <div className="d-flex justify-content-center flex-column">
        <div className="table-container">
          <table className="data-table w-100" style={{ minWidth: "500px" }}>
            <thead>
              <tr>
                <th
                  style={{ width: "20%", minWidth: "100px" }}
                  className="text-center pt-3 pb-3"
                >
                  Table Name
                </th>
                <th
                  style={{ width: "20%", minWidth: "100px" }}
                  className="text-center pt-3 pb-3"
                >
                  Rows in Table
                </th>
                <th
                  style={{ width: "20%", minWidth: "100px" }}
                  className="text-center pt-3 pb-3"
                >
                  Delete Table
                </th>
                <th
                  style={{ width: "20%", minWidth: "100px" }}
                  className="text-center pt-3 pb-3"
                >
                  Modify Table
                </th>
                <th
                  style={{ width: "20%", minWidth: "100px" }}
                  className="text-center pt-3 pb-3"
                >
                  Access Table Data
                </th>
              </tr>
            </thead>
            <tbody>
              {tableData.map((row, index) => (
                <tr key={index}>
                  <td className="text-center">{row.tableName}</td>
                  <td className="text-center">{row.rowsInTable}</td>
                  <td className="text-center">
                    <button
                      className="btn btn-primary m-1"
                      onClick={() => {
                        setModalOpen(true);
                        setToDelete(index);
                      }}
                    >
                      Delete
                    </button>
                  </td>
                  <td className="text-center">
                    <button
                      className="btn btn-primary m-1"
                      onClick={() => {
                        alertService.warning(
                          "What's up doc? Whatever it is, its not this feature."
                        );
                      }}
                    >
                      Modify
                    </button>
                  </td>
                  <td className="text-center">
                    <button
                      className="btn btn-primary m-1"
                      onClick={() => {
                        switchView(row);
                      }}
                    >
                      Access
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
