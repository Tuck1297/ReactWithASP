const Modal = ({ message, btnActionName, setModalOpen, modalOpen, btnAction }) => {
  const modalClose = () => {
    setModalOpen(false);
  };

  return (
    <div
      className={`modal fade ${modalOpen ? "show d-block" : "d-hidden"}`}
      tabIndex="-1"
      role="dialog"
      aria-labelledby="exampleModalLabel"
      aria-hidden={modalOpen}
      style={{ backgroundColor: "rgba(0,0,0,0.5)" }}
    >
      <div className="modal-dialog" role="document">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">{btnActionName}</h5>
            <button
              type="button"
              className="close"
              data-dismiss="modal"
              aria-label="Close"
              onClick={modalClose}
              style={{ background: "rgba(0,0,0,0)", border: "none" }}
            >
              <svg
                width="25px"
                height="25px"
                fill="none"
                stroke="black"
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2px"
                viewBox="0 0 24 24"
              >
                <path d="M18 6 6 18" />
                <path d="m6 6 12 12" />
              </svg>
            </button>
          </div>
          <div className="modal-body">
            <p>{message}</p>
          </div>
          <div className="modal-footer">
            <button
              type="button"
              className="btn btn-secondary"
              data-dismiss="modal"
              onClick={modalClose}
            >
              Close
            </button>
            <button type="button" className="btn btn-danger" onClick={btnAction}>
              {btnActionName}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Modal;
