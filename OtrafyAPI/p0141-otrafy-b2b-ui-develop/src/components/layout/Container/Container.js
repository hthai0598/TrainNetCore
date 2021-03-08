import React from "react"
import PropTypes from "prop-types"

export const Container = ({maxWidth = 1170, children, horizontalPadding = 15, verticalPadding = 0}) => {

  return (
      <div style={{
        marginLeft: "auto",
        marginRight: "auto",
        padding: `${verticalPadding}px ${horizontalPadding}px`,
        maxWidth: `${maxWidth + horizontalPadding * 2}px`
      }}>
        {children}
      </div>
  )
}

Container.propTypes = {
  maxWidth: PropTypes.number,
  horizontalPadding: PropTypes.number,
  verticalPadding: PropTypes.number,
  children: PropTypes.node.isRequired
}

export const FlexContainer = ({maxWidth = 1170, justify = "unset", alignItems = "unset", alignContents = "unset", direction = "unset", horizontalPadding = 15, verticalPadding = 0, children}) => {

  return (
      <div style={{
        maxWidth: `${maxWidth + horizontalPadding * 2}px`,
        padding: `${verticalPadding}px ${horizontalPadding}px`,
        marginLeft: "auto",
        marginRight: "auto",
        display: "flex",
        flexWrap: "wrap",
        justifyContent: `${justify}`,
        alignItems: `${alignItems}`,
        alignContent: `${alignContents}`,
        flexDirection: `${direction}`
      }}>
        {children}
      </div>
  )
}

FlexContainer.propTypes = {
  maxWidth: PropTypes.number,
  children: PropTypes.node.isRequired,
  justify: PropTypes.oneOf([
    "space-between",
    "space-around",
    "flex-start",
    "flex-end",
    "center"
  ]),
  alignItems: PropTypes.oneOf([
    "center",
    "stretch",
    "flex-end",
    "flex-start"
  ]),
  alignContents: PropTypes.oneOf([
    "center",
    "stretch",
    "flex-end",
    "flex-start"
  ]),
  direction: PropTypes.oneOf([
    "row",
    "row-reverse",
    "column",
    "column-reverse"
  ])
}
