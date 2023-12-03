import React, { useState, useEffect } from "react";
import Modal from "../../Modal";
import { alertService } from "../../../services/alertService";
import { externalDbService } from "../../../services/externalDbService";
import SmallSpinner from "../../loading/SmallSpinner";
import CenterElement from "../../bootstrap/CenterElement";
import ArrowLeft from "../../icons/ArrowLeft";
import ArrowRight from "../../icons/ArrowRight";
import LargeSpinner from "../../loading/LargeSpinner";

const PAGE_SIZE_FOR_DATA = 25; // 25 Rows per page

// NOTE: Initial data load executes in parent due to actions relying on that
// data load. Remaining data loads execute in child where actions rely on data load.

const Table = ({
  data = [],
  currentDBInteracting,
  currentTableInteracting,
}) => {
  const [tableData, setTableData] = useState(data);
  const [toDisplayData, setToDisplayData] = useState(data);
  const [newRow, setNewRow] = useState({});
  const [editRow, setEditRow] = useState(null);
  const [editRowData, setEditRowData] = useState(null);
  const [modalOpen, setModalOpen] = useState(false);
  const [toDelete, setToDelete] = useState(null);
  const [addLoading, setAddLoading] = useState(false);
  const [updateLoading, setUpdateLoading] = useState(false);
  const [deleteLoading, setDeleteLoading] = useState(false);
  const [newDataLoading, setNewDataLoading] = useState(false);
  const [index, setIndex] = useState(0);
  const [maxPages, setMaxPages] = useState(
    Math.ceil(currentTableInteracting.rowsInTable / PAGE_SIZE_FOR_DATA)
  );

  function findObjInArrayIndex(targetObj, array) {
    const index = array.findIndex((obj) => {
      return JSON.stringify(obj) === JSON.stringify(targetObj);
    });
    return index;
  }

  const handleUpdate = (index) => {
    const oldRowData = toDisplayData[index];
    const newRowData = editRowData;
    setUpdateLoading(true);
    setTimeout(async () => {
      await externalDbService
        .updateRow(currentDBInteracting, oldRowData, newRowData)
        .then((result) => {
          alertService.success("Row updated successfully.");
          // Update global stored data
          const updatedData = [...tableData];
          const updateIndex = findObjInArrayIndex(oldRowData, tableData);
          updatedData[updateIndex] = { ...updatedData[updateIndex], ...editRowData };
          setTableData(updatedData);
          // Update data shown to user
          const updatedToDisplayData = [...toDisplayData];
          const updateToDisplayIndex = findObjInArrayIndex(oldRowData, toDisplayData);
          updatedToDisplayData[updateToDisplayIndex] = {...updatedToDisplayData[updateToDisplayIndex], ...editRowData};
          setToDisplayData(updatedToDisplayData);
          setEditRow(null);
          setUpdateLoading(false);
        })
        .catch((error) => {
          console.log(error)
          alertService.error(error);
          setUpdateLoading(false);
        });
    }, 1000);
  };

  const handleDelete = (deleteData) => {
    setDeleteLoading(true);
    console.log(toDelete, deleteData)
    setTimeout(async () => {
      await externalDbService
        .deleteRow(currentDBInteracting, deleteData)
        .then((result) => {
          alertService.success("Row deleted successfully.");
          // Update global data stored
          const updatedData = [...tableData];
          const index = findObjInArrayIndex(deleteData, updatedData);
          updatedData.splice(index, 1);
          setTableData(updatedData);
          // Update data currently displayed to user
          const updatedToDisplayData = [...toDisplayData];
          const index2 = findObjInArrayIndex(deleteData, toDisplayData);
          updatedToDisplayData.splice(index2, 1);
          setToDisplayData(updatedToDisplayData);
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
          setTableData([...tableData, newRow]);
          setToDisplayData([...toDisplayData, newRow]);
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

  const updateView = (newIndex) => {
    if (newIndex < 0) return;
    if (newIndex >= maxPages) return;
    setIndex(newIndex);

    // example: 0 * 25 = 0 & 1 * 25 = 25 (0,25) (25, 50)...
    // So when newIndex is equal to 7 the result would be 25 * 7 & 25 * 8 equals (175, 200)
    const startRangeNeeded = newIndex * PAGE_SIZE_FOR_DATA; // will never be less then 0
    const endRangeNeeded = (newIndex + 1) * PAGE_SIZE_FOR_DATA; // could be larger then actual all data retrieved

    const currentSizeOfData = tableData.length;

    if (
      startRangeNeeded <= currentSizeOfData &&
      endRangeNeeded <= currentSizeOfData
    ) {
      // already have data loaded so update data view property and settimeout after
      // doing load animation
      console.log("DONT NEED NEW DATA");
      setNewDataLoading(true);
      setTimeout(() => {
        setToDisplayData(tableData.slice(startRangeNeeded, endRangeNeeded));
        setNewDataLoading(false);
      }, 2000);
    } else {
      // need to load data
      console.log("LOAD DATA...");
      setNewDataLoading(true);
      setTimeout(() => {
        externalDbService
          .getDataFromTable(
            currentDBInteracting,
            startRangeNeeded,
            PAGE_SIZE_FOR_DATA
          )
          .then((result) => {
            const updatedData = [...tableData, ...result];
            setTableData(updatedData);
            setToDisplayData(
              updatedData.slice(
                startRangeNeeded,
                Math.min(endRangeNeeded, updatedData.length)
              )
            );
            setNewDataLoading(false);
          })
          .catch((error) => {});
      }, 2000);
    }
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

  const handleUpdateToEdit = (index, data) => {
    setEditRow(index);
    setEditRowData(data);
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
            {!newDataLoading ? (
              <>
                <tbody>
                  {toDisplayData.map((row, index) => (
                    <tr key={index} className="">
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
                              onClick={() => handleUpdateToEdit(index, row)}
                              disabled={editRow != null || deleteLoading}
                            >
                              Edit
                            </button>
                            <button
                              className="btn btn-primary m-1"
                              onClick={() => {
                                setModalOpen(true);
                                setToDelete(row);
                              }}
                              disabled={editRow != null || deleteLoading}
                            >
                              {deleteLoading && toDelete === row ? (
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
              </>
            ) : (
              ""
            )}
          </table>
          {newDataLoading ? (
            <section className="w-100" style={{ height: "60vh" }}>
              <CenterElement>
                <div className="d-flex justify-content-center flex-column align-items-center">
                  <LargeSpinner />
                  <h2 className="text-center mt-3">Loading...</h2>
                </div>
              </CenterElement>
            </section>
          ) : (
            ""
          )}
          <h4 className="text-center w-100">
            {tableData.length === 0 ? "Nothing to see here..." : ""}
          </h4>
        </div>
        <section>
            <CenterElement>
              <button
                className="btn btn-secondary m-1"
                onClick={() => {
                  updateView(index - 1);
                }}
                disabled={index === 0 || newDataLoading}
              >
                <ArrowLeft />
              </button>
              <button
                className="btn btn-secondary m-1"
                onClick={() => {
                  updateView(index + 1);
                }}
                disabled={index === maxPages - 1 || newDataLoading}
              >
                <ArrowRight />
              </button>
            </CenterElement>
          </section>
      </div>
    </>
  );
};

export default Table;
