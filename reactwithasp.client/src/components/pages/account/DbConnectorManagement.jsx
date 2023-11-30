import ObjectTable from "../../forms/tables/DbDataTable";
import DbConnectionsSummaryTable from "../../forms/tables/DbConnectionsSummaryTable";
import DbTableSummary from "../../forms/tables/DbTableSummary";
import LargeSpinner from "../../loading/LargeSpinner";
import { useEffect, useContext, useState } from "react";
import { userService } from "../../../services/userService";
import { alertService } from "../../../services/alertService";
import { useNavigate } from "react-router-dom";
import { UserAuthContext } from "../../UserAuthContext";
import CenterElement from "../../bootstrap/CenterElement";
import { dbCSService } from "../../../services/dbCSService";
import { externalDbService } from "../../../services/externalDbService";

/* TODOS:
    Create back button to go t previous view
    Create a reset button that will essentially refresh the page, dump all data and restart
    need module here for deleting cs...
*/

const tableSummary = [
  { tableName: "test1", rowsInTable: 22 },
  { tableName: "test2", rowsInTable: 2 },
  { tableName: "test3", rowsInTable: 44 },
  { tableName: "test4", rowsInTable: 63 },
];

const tableTestData = [
  {
    id: 1,
    name: "John Doe",
    address: "123 Main St, Cityville",
    age: 25,
    moneyInBank: 5000,
    gender: "Male",
    favoriteFood: "Pizza",
  },
  {
    id: 2,
    name: "Jane Doe",
    address: "456 Oak St, Townsville",
    age: 30,
    moneyInBank: 8000,
    gender: "Female",
    favoriteFood: "Sushi",
  },
  {
    id: 3,
    name: "Bob Smith",
    address: "789 Pine St, Villagetown",
    age: 22,
    moneyInBank: 3000,
    gender: "Male",
    favoriteFood: "Burger",
  },
];

const ConnectionStringManagementPage = () => {
  const navigate = useNavigate();
  const { signedIn, setSignedIn } = useContext(UserAuthContext);
  const [loading, setLoadingState] = useState(true);
  const [currentDataDisplay, setcurrentDataDisplay] = useState({
    db: true,
    tables: false,
    table_data: false,
  });
  const [displayScreenMessage, setScreenMessage] = useState(
    "Loading saved database connections."
  );
  const [loadedDBs, setLoadedDBs] = useState(null);
  const [loadedTablesFromDB, setLoadedTablesFromDB] = useState(null);
  const [loadedDataFromTable, setLoadedDataFromTable] = useState(null);

  function switchToTablesView(data) {
    console.log(data);
    setLoadingState(true);
    setcurrentDataDisplay({ ...currentDataDisplay, db: false });
    setScreenMessage("Loading available tables in selected database.");

    setTimeout(async () => {
      await externalDbService
        .getAllTableNames(data.id)
        .then((result) => {
          console.log(result);
          setLoadedTablesFromDB(result);
          setScreenMessage(null);
          setcurrentDataDisplay({
            ...currentDataDisplay,
            tables: true,
            db: false,
          });
          setLoadingState(false);
        })
        .catch((error) => {
          setScreenMessage(
            "There was a problem loading this information. Please try again later."
          );
        });
    }, 1000);

    // once data is loaded - set loading state to false, currentdisplay for tables to true and screen message to null
    // setTimeout(() => {
    //   setLoadedTablesFromDB(tableSummary);
    //   setScreenMessage(null);
    //   setcurrentDataDisplay({ ...currentDataDisplay, tables: true, db: false });
    //   setLoadingState(false);
    // }, 1000);
  }

  function switchToTableDataView(data) {
    console.log(data);
    setLoadingState(true);
    setcurrentDataDisplay({ ...currentDataDisplay, tables: false });
    setScreenMessage("Loading data for selected database table.");

    // once data is loaded - set laoding state to false, currrentdisplay for table_data to true and screen message to null
    setTimeout(() => {
      setLoadedDataFromTable(tableTestData);
      setScreenMessage(null);
      setcurrentDataDisplay({
        ...currentDataDisplay,
        tables: false,
        table_data: true,
      });
      setLoadingState(false);
    }, 1000);
  }

  async function getDbConnectorNames() {
    setTimeout(async () => {
      await dbCSService
        .getDBNames()
        .then((result) => {
          setLoadedDBs(result);
          setLoadingState(false);
          setScreenMessage(null);
        })
        .catch(() => {
          setScreenMessage(
            "There was a problem loading this information. Please try again later."
          );
        });
    }, 1000);
  }

  function backBasedOnState() {
    if (currentDataDisplay.db) {
      navigate("/account/home");
      return;
    }
    if (currentDataDisplay.tables) {
      setLoadingState(true);
      setcurrentDataDisplay({ ...currentDataDisplay, tables: false });
      setScreenMessage("Loading saved database connections.");
      // once data is loaded - set laoding state to false, currrentdisplay for table_data to true and screen message to null
      setTimeout(() => {
        setScreenMessage(null);
        setcurrentDataDisplay({
          ...currentDataDisplay,
          tables: false,
          db: true,
        });
        setLoadingState(false);
      }, 1000);
      return;
    }
    if (currentDataDisplay.table_data) {
      setLoadingState(true);
      setcurrentDataDisplay({ ...currentDataDisplay, table_data: false });
      setScreenMessage("Loading tables from database.");

      // once data is loaded - set laoding state to false, currrentdisplay for table_data to true and screen message to null
      setTimeout(() => {
        setScreenMessage(null);
        setcurrentDataDisplay({
          ...currentDataDisplay,
          tables: true,
          table_data: false,
        });
        setLoadingState(false);
      }, 1000);
      return;
    }
  }

  useEffect(() => {
    getDbConnectorNames();
  }, []);

  return (
    <section className="page">
      <div>
        {loading ? (
          <CenterElement className="mt-5">
            <LargeSpinner />
          </CenterElement>
        ) : (
          <>
            <CenterElement>
              <button
                onClick={backBasedOnState}
                className="btn btn-secondary mt-2"
              >
                Back
              </button>
            </CenterElement>
            {currentDataDisplay.db ? (
              <DbConnectionsSummaryTable
                data={loadedDBs}
                switchView={switchToTablesView}
              />
            ) : (
              ""
            )}
            {currentDataDisplay.tables ? (
              <DbTableSummary
                data={loadedTablesFromDB}
                switchView={switchToTableDataView}
              />
            ) : (
              ""
            )}
            {currentDataDisplay.table_data ? (
              <ObjectTable data={loadedDataFromTable} />
            ) : (
              ""
            )}
          </>
        )}
        <h4 className="text-center mt-5">{displayScreenMessage}</h4>
      </div>
    </section>
  );
};

export default ConnectionStringManagementPage;
