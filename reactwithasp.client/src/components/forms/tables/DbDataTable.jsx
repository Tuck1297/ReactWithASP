import React, { useState } from "react";
import Modal from "../../Modal";
import { alertService } from "../../../services/alertService";
import { externalDbService } from "../../../services/externalDbService";
import SmallSpinner from "../../loading/SmallSpinner";

const Table = ({ data, currentDBInteracting }) => {
  const [tableData, setTableData] = useState(data);
  const [newRow, setNewRow] = useState({});
  const [editRow, setEditRow] = useState(null);
  const [editRowData, setEditRowData] = useState(null);
  const [modalOpen, setModalOpen] = useState(false);
  const [toDelete, setToDelete] = useState(null);
  const [addLoading, setAddLoading] = useState(null);
  const [updateLoading, setUpdateLoading] = useState(null);
  const [deleteLoading, setDeleteLoading] = useState(null);

  const handleUpdate = (index) => {
    const oldRowData = tableData[index];
    const newRowData = editRowData;
    setUpdateLoading(true);
    setTimeout(async () => {
      await externalDbService
        .updateRow(currentDBInteracting, oldRowData, newRowData)
        .then((result) => {
          alertService.success("Row updated successfully.");
          const updatedData = [...tableData];
          updatedData[index] = { ...updatedData[index], ...editRowData };
          setTableData(updatedData);
          setEditRow(null);
          setUpdateLoading(false);
        })
        .catch((error) => {
          alertService.error(error);
          setUpdateLoading(false);
        });
    }, 1000);
  };

  const handleDelete = (index) => {
    const deleteParams = tableData[index];
    setDeleteLoading(true);
    console.log(index, toDelete, deleteLoading);

    setTimeout(async () => {
      await externalDbService
        .deleteRow(currentDBInteracting, deleteParams)
        .then((result) => {
          alertService.success("Row deleted successfully.");
          const updatedData = [...tableData];
          updatedData.splice(index, 1);
          setTableData(updatedData);
          setDeleteLoading(false);
          setToDelete(null);
        })
        .catch((error) => {
          alertService.error(error);
          setDeleteLoading(false);
          setToDelete(null);
        });
    }, 1000);
  };

  const handleAddRow = () => {
    if (!validateObjectKeys(tableData[0], newRow)) {
      return;
    }
    const newRowData = newRow;
    setAddLoading(true);
    setTimeout(async () => {
      await externalDbService
        .createRow(currentDBInteracting, newRowData)
        .then((result) => {
          alertService.success("Row added successfully!");
          if (tableData.length == 0) {
            setTableData([newRow]);
            setNewRow({});
            return;
          }
          setTableData([newRow, ...tableData]);
          setNewRow({});
          setAddLoading(false);
        })
        .catch((error) => {
          console.error(error);
          alertService.error(error);
          setAddLoading(false);
        });
    }, 1000);
  };

  function validateObjectKeys(sourceObject, targetObject) {
    // Loop through all keys in the sourceObject
    for (const key of Object.keys(sourceObject)) {
      // Check if the key is present in the targetObject
      if (!(key in targetObject)) {
        alertService.warning(`New column "${key}" is empty.`);
        return false;
      }

      // Check if the corresponding value is not null or an empty string
      const targetValue = targetObject[key].trim();
      if (targetValue === null || targetValue === "") {
        alertService.warning(`New column "${key}" is empty.`);
        return false;
      }
    }
    // All keys are present and values are not null or empty
    return true;
  }

  if (!tableData) {
    return;
  }
  if (tableData.length === 0) {
    return (
      <>
        <h4 className="text-center w-100 mt-5">Nothing to see here...</h4>
      </>
    );
  }

  const handleUpdateToEdit = (index) => {
    setEditRow(index);
    setEditRowData(tableData[index]);
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
                <th
                  className="text-center pt-3 pb-3"
                  style={{ width: `${percentage}%`, minWidth: "100px" }}
                >
                  Actions
                </th>
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
                          className="form-control"
                          onChange={(e) => {
                            e.preventDefault();
                            setEditRowData({
                              ...editRowData,
                              [key]: e.target.value,
                            });
                          }}
                        />
                      ) : (
                        <>
                          {row[key].length >= 150
                            ? row[key].toString().slice(0, 100) + "..."
                            : row[key].toString()}
                        </>
                      )}
                    </td>
                  ))}
                  <td className="d-flex justify-content-center align-items-center">
                    {editRow === index ? (
                      <>
                        <button
                          className="btn btn-primary m-1"
                          onClick={() => handleUpdate(index)}
                          disabled={updateLoading}
                        >
                          {updateLoading ? <SmallSpinner /> : "Update"}
                        </button>
                        <button
                          className="btn btn-primary m-1"
                          onClick={() => {
                            setEditRow(null);
                          }}
                          disabled={updateLoading}
                        >
                          Cancel
                        </button>
                      </>
                    ) : (
                      <>
                        <button
                          className="btn btn-primary m-1"
                          onClick={() => handleUpdateToEdit(index)}
                          disabled={editRow != null || deleteLoading}
                        >
                          Edit
                        </button>
                        <button
                          className="btn btn-primary m-1"
                          onClick={() => {
                            setModalOpen(true);
                            setToDelete(index);
                          }}
                          disabled={editRow != null || deleteLoading}
                        >
                          {deleteLoading && toDelete === index ? (
                            <SmallSpinner />
                          ) : (
                            "Delete"
                          )}
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
                      disabled={
                        editRow != null ||
                        addLoading ||
                        deleteLoading ||
                        updateLoading
                      }
                    />
                  </td>
                ))}
                <td className="d-flex justify-content-center align-items-center">
                  <button
                    className="btn btn-primary m-1"
                    onClick={handleAddRow}
                    disabled={
                      editRow != null ||
                      addLoading ||
                      deleteLoading ||
                      updateLoading
                    }
                  >
                    {addLoading ? <SmallSpinner /> : "Insert"}
                  </button>
                </td>
              </tr>
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
