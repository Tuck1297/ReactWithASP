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
    Create a reset button that will essentially refresh the page, dump all data and restart
*/

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
  const [currentDBInteracting, setCurrentDBInteracting] = useState(null);

  function switchToTablesView(data) {
    setLoadingState(true);
    setcurrentDataDisplay({ ...currentDataDisplay, db: false });
    setScreenMessage("Loading available tables in selected database.");
    setCurrentDBInteracting(data.id);

    setTimeout(async () => {
      await externalDbService
        .getAllTableNames(data.id)
        .then((result) => {
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
  }

  function switchToTableDataView(data) {
    console.log(data);
    setLoadingState(true);
    setcurrentDataDisplay({ ...currentDataDisplay, tables: false });
    setScreenMessage("Loading data for selected database table.");

    setTimeout(async () => {
      await externalDbService
        .updateTableInteracting(data.tableName, currentDBInteracting)
        .then((result) => {
          console.log("table interacting", result);
          return externalDbService.getDataFromTable(
            currentDBInteracting,
            0,
            25
          );
        })
        .then((result) => {
          setLoadedDataFromTable(result);
          setScreenMessage(null);
          setcurrentDataDisplay({
            ...currentDataDisplay,
            tables: false,
            table_data: true,
          });
          setLoadingState(false);
        })
        .catch((error) => {
          console.error(error);
          setScreenMessage(
            "There was a problem loading this information. Please try again later."
          );
        });
    }, 1000);
  }

  function getDbConnectorNames() {
    setScreenMessage("Loading Database Connections...");
    setTimeout(async () => {
      await dbCSService
        .getDBNames()
        .then((result) => {
          setLoadedDBs(result);
          setLoadingState(false);
          setScreenMessage(null);
          setcurrentDataDisplay({...currentDataDisplay, db: true});
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
      // once data is loaded - set loading state to false, currrentdisplay for table_data to true and screen message to null
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

  function resetView() {
    setLoadedTablesFromDB(null);
    setLoadedDataFromTable(null);
    setCurrentDBInteracting(null);
    setLoadedDBs(null);
    setScreenMessage("Restarting and grabbing some Mt. Dew...");
    setLoadingState(true);
    setcurrentDataDisplay({
      db: false,
      tables: false,
      table_data: false,
    });
    setTimeout(() => {
      getDbConnectorNames();
    }, 2000);
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
            <CenterElement className="flex-row">
              <button
                onClick={backBasedOnState}
                className="btn btn-secondary mt-2"
              >
                Back
              </button>
              <button
                onClick={resetView}
                className="btn btn-secondary mt-2"
              >
                Reset
              </button>
            </CenterElement>
            {currentDataDisplay.db ? (
              <DbConnectionsSummaryTable
                data={loadedDBs}
                switchView={switchToTablesView}
                currentDBInteracting={currentDBInteracting}
              />
            ) : (
              ""
            )}
            {currentDataDisplay.tables ? (
              <DbTableSummary
                data={loadedTablesFromDB}
                switchView={switchToTableDataView}
                currentDBInteracting={currentDBInteracting}
              />
            ) : (
              ""
            )}
            {currentDataDisplay.table_data ? (
              <ObjectTable
                data={loadedDataFromTable}
                currentDBInteracting={currentDBInteracting}
              />
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
