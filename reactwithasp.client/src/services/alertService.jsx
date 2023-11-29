import { BehaviorSubject } from "rxjs";

const alertSubject = new BehaviorSubject(null);

export const alertService = {
    alert: alertSubject.asObservable(),
    success,
    error,
    warning,
    clear
};

function success(message, showAfterRedirect = false) {
    alertSubject.next({
        type: 'alert-success',
        message,
        showAfterRedirect
    });
}

function error(message, showAfterRedirect = false) {
    alertSubject.next({
        type: 'alert-danger',
        message,
        showAfterRedirect
    })
}

function warning(message, showAfterRedirect = false) {
    alertSubject.next({
        type: 'alert-warning',
        message,
        showAfterRedirect
    })
}

function clear() {
    // if showAfterRedirect is true the alert is not cleared
    // for one route change
    let alert = alertSubject.value;
    if (alert?.showAfterRedirect) {
        alert.showAfterRedirect = false;
    } else {
        alert = null;
    }
    alertSubject.next(alert);
}