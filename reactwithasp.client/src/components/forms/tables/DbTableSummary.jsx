import React, { useState } from "react";
import Modal from "../../Modal";
import { alertService } from "../../../services/alertService";

const Table = ({ data, switchView }) => {
  const [tableData, setTableData] = useState(data);
  const [newRow, setNewRow] = useState({});
  const [editRow, setEditRow] = useState(null);
  const [editRowData, setEditRowData] = useState(null);
  const [modalOpen, setModalOpen] = useState(false);
  const [toDelete, setToDelete] = useState(null);

  // TODOs will be sections that will connect with api connection of database.

  // TODO: this page will display the databases's tables, number of rows in each table, all columns in each table, ability to update the name or delete a column in a table
  // ability to delete a table and ability to access data within a particular table (third view)

  const handleDelete = (index) => {
    // TODO - connect database delete action here
    const updatedData = [...tableData];
    updatedData.splice(index, 1);
    setTableData(updatedData);
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
                        alertService.warning("What's up doc? Whatever it is, its not this feature.");
                      }}
                    >
                      Modify
                    </button>
                  </td>
                  <td className="text-center">
                    <button
                      className="btn btn-primary m-1"
                      onClick={() => {
                        console.log(row)
                        switchView(row)
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
