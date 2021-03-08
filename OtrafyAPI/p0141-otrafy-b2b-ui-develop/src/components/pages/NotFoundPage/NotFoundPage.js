import React from 'react'
import { Link } from 'react-router-dom'
import spacetime from 'spacetime'
import { Helmet } from 'react-helmet'
import { inject, observer } from 'mobx-react'
import { Wrapper } from './CustomStyled'

const NotFoundPage = ({ commonStore }) => {

  return (
    <Wrapper>
      <Helmet>
        <title>404 | Website</title>
      </Helmet>
      <div className="content">
        <div className="browser-bar">
          <span className="close button"/>
          <span className="min button"/>
          <span className="max button"/>
        </div>
        <div className="text">
          <p>
            Last login: {spacetime.now().format('nice')}
          </p>
          <p>
            -bash: Oops! The page you're looking for was not found. Go back <Link to={'/'}
                                                                                  style={{ color: commonStore.appTheme.solidColor }}>home</Link> and
            start over.
          </p>
          <p>Users-MBP:~ user$ <span className="indicator"/></p>
        </div>
      </div>
    </Wrapper>
  )
}

export default inject('commonStore')(observer(NotFoundPage))
