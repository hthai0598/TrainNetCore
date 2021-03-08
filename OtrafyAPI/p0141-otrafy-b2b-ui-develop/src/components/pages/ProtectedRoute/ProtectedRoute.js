import React from "react"
import {Redirect, Route} from "react-router-dom"

const ProtectedRoute = ({component: Component, ...rest}) => (
    <Route {...rest} render={props => (
        localStorage.getItem("jwt") || sessionStorage.getItem("jwt")
        ? <Component {...props} />
        : <Redirect to={{
          pathname: "/login",
          state: {from: props.location}
        }}/>
    )}/>
)

export default ProtectedRoute
