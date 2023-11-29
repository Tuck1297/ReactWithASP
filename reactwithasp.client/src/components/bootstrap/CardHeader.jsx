const CardHeader = ({ className = "", children, ...props }) => {
  return <div className={`card-header ${className}`} {...props}>{children}</div>;
};
export default CardHeader;