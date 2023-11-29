import CardHeader from "./CardHeader";
import CardBody from "./CardBody";

const Card = ({ className = "", children, header, ...props }) => {
  return (
    <div className={`card ${className}`} {...props}>
      <CardHeader className="text-center fs-4">{header}</CardHeader>
      <CardBody>{children}</CardBody>
    </div>
  );
};
export default Card;
