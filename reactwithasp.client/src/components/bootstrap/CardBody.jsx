const CardBody = ({className = "", children, ...props}) => {
    return <div className={`card-body p-2 ${className}`} {...props}>{children}</div>
}
export default CardBody;