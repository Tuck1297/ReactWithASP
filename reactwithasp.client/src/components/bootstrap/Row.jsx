const Row = ({ children, className = "", ...props }) => {
    const combinedClasses = `row w-100 h-100 ${className}`;
    return (
      <div className={combinedClasses} {...props}>
        {children}
      </div>
    );
  };
  export default Row;