import React, { useState } from "react";
import Modal from "../../Modal";
import { alertService } from "../../../services/alertService";

const Table = ({ data }) => {
  const [tableData, setTableData] = useState(data);
  const [newRow, setNewRow] = useState({});
  const [editRow, setEditRow] = useState(null);
  const [editRowData, setEditRowData] = useState(null);
  const [modalOpen, setModalOpen] = useState(false);
  const [toDelete, setToDelete] = useState(null);

  const handleUpdate = (index) => {
    // TODO - Connect database update action here
    const updatedData = [...tableData];
    updatedData[index] = { ...updatedData[index], ...editRowData };
    setTableData(updatedData);
    setEditRow(null);
  };

  const handleDelete = (index) => {
    // TODO - connect database delete action here
    const updatedData = [...tableData];
    updatedData.splice(index, 1);
    setTableData(updatedData);
  };

  const handleAddRow = () => {
    if (tableData.length == 0) {
      setTableData([newRow]);
      setNewRow({});
      return;
    }
    // TODO - connection database add action here
    if (!validateObjectKeys(tableData[0], newRow)) {
      return;
    }
    
    setTableData([newRow, ...tableData]);
    setNewRow({});
  };

  function validateObjectKeys(sourceObject, targetObject) {
    // Loop through all keys in the sourceObject
    for (const key of Object.keys(sourceObject)) {
      // Check if the key is present in the targetObject
      if (!(key in targetObject)) {
        alertService.warning(`New column "${key}" is empty.`)
        return false;
      }

      // Check if the corresponding value is not null or an empty string
      const targetValue = targetObject[key].trim();
      if (targetValue === null || targetValue === "") {
        alertService.warning(`New column "${key}" is empty.`)
        return false;
      }
    }
    // All keys are present and values are not null or empty
    return true;
  }

  const handleUpdateToEdit = (index) => {
    setEditRow(index);
    setEditRowData(data[index]);
  };
  const numberOfNeededCols = Object.keys(data[0]).length + 1;
  let percentage = 100 / numberOfNeededCols;
  if (numberOfNeededCols > 8) {
    percentage = 0;
  }

  return (
    <>
      <Modal
        message="Are you sure you want to delete this row? After this point changes cannot be reverted!"
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
                {Object.keys(data[0]).map((key) => (
                  <th
                    key={key}
                    className="text-center pt-3 pb-3"
                    style={{ textTransform: "capitalize", width: `${percentage}%`, minWidth: "100px" }}
                  >
                    {key}
                  </th>
                ))}
                <th className="text-center pt-3 pb-3">Actions</th>
              </tr>
            </thead>
            <tbody>
              {tableData.map((row, index) => (
                <tr key={index}>
                  {Object.keys(row).map((key) => (
                    <td key={key} className="text-center pt-3 pb-3">
                      {editRow === index ? (
                        <input
                          type="text"
                          value={editRowData[key]}
                          onChange={(e) => {
                            e.preventDefault();
                            setEditRowData({
                              ...editRowData,
                              [key]: e.target.value,
                            });
                          }}
                        />
                      ) : (
                        row[key]
                      )}
                    </td>
                  ))}
                  <td className="d-flex justify-content-center align-items-center">
                    {editRow === index ? (
                      <button
                        className="btn btn-primary m-1"
                        onClick={() => handleUpdate(index)}
                      >
                        Update
                      </button>
                    ) : (
                      <>
                        <button
                          className="btn btn-primary m-1"
                          onClick={() => handleUpdateToEdit(index)}
                          disabled={editRow != null}
                        >
                          Edit
                        </button>
                        <button
                          className="btn btn-primary m-1"
                          onClick={() => {
                            setModalOpen(true);
                            setToDelete(index);
                          }}
                          disabled={editRow != null}
                        >
                          Delete
                        </button>
                      </>
                    )}
                  </td>
                </tr>
              ))}
              <tr>
                {Object.keys(data[0]).map((key) => (
                  <td key={key} className="text-center pt-3 pb-3">
                    <input
                      className="form-control"
                      type="text"
                      placeholder={key.toUpperCase()}
                      value={newRow[key] || ""}
                      onChange={(e) =>
                        setNewRow({ ...newRow, [key]: e.target.value })
                      }
                    />
                  </td>
                ))}
                <td className="d-flex justify-content-center align-items-center">
                  <button
                    className="btn btn-primary m-1"
                    onClick={handleAddRow}
                  >
                    Add Row
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
          <h4 className="text-center w-100">{tableData.length === 0 ? "Nothing to see here..." : ""}</h4>
        </div>
      </div>
    </>
  );
};

export default Table;
