import { useEffect, useState } from "react";
import { alertService } from "../services/alertService";

function Alert() {
  const [alert, setAlert] = useState(null);

    function clearAlert() {
        console.log(alert)
        console.log("clear alert")
        alertService.clear();
    }

  useEffect(() => {
    const subscription = alertService.alert.subscribe((alert) => {
      if (Array.isArray(alert?.message)) {
        alert.message = alert.message.join(", ");
      } else if (
        typeof alert?.message === "object" &&
        alert?.message !== null
      ) {
        const values = Object.values(alert.message);
        alert.message = values[0];
      } else if (typeof alert?.message === "string") {
        // default type don't need to do anything else
      }
      setAlert(alert);
    });

    // unsubscribe when the component unmounts
    return () => subscription.unsubscribe();
  });
  if (!alert) return null;
  const combinedClasses = `alert ${alert.type} alert-dismissible fade show m-0`
  return (
    <div className={combinedClasses} role="alert">
      <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" className="bi bi-exclamation-triangle-fill flex-shrink-0 me-2" viewBox="0 0 16 16" role="img" aria-label="Warning:">
    <path d="M8.982 1.566a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767L8.982 1.566zM8 5c.535 0 .954.462.9.995l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 5.995A.905.905 0 0 1 8 5zm.002 6a1 1 0 1 1 0 2 1 1 0 0 1 0-2z"/>
  </svg>
      {alert.message}
      <button
        type="button"
        className="btn-close"
        data-bs-dismiss="alert"
        aria-label="Close"
        onClick={clearAlert}
      ></button>
    </div>
  );
}
export default Alert;
