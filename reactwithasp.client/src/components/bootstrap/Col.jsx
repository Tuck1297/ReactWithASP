const Col = ({
  children,
  className = "",
  ColNumSize = "6",
  ColSize = "md",
  ...props
}) => {
  const combinedClasses = `col-${ColSize}-${ColNumSize} ${className}`;
  return (
    <div className={combinedClasses} {...props}>
      {children}
    </div>
  );
};

export default Col;
