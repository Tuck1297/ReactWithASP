const CenterElement = ({ className = "", children, ...props }) => {
  return (
    <div
      className={`${className} d-flex justify-content-center align-items-center h-100`}
      {...props}
    >
      {children}
    </div>
  );
};
export default CenterElement;