import React from 'react'
import { Form, Input, Button } from 'antd'
import { Helmet } from 'react-helmet'
import validator from '../../../validator'
import { inject, observer } from 'mobx-react'
import { withRouter } from 'react-router-dom'
import {
  Heading,
  Message,
  NaviLink,
} from './CustomStyled'

const ForgotPasswordForm = ({ authStore, history }) => {

  const CustomForgotPasswordForm = props => {

    const { getFieldDecorator } = props.form

    const handleSubmit = e => {
      e.preventDefault()
      props.form.validateFields((err, values) => {
        if (!err) {
          authStore.userForgotPassword(values.email, history)
        }
      })
    }

    return (
      <Form onSubmit={handleSubmit} className={'forgot_password-form'}>
        <Form.Item label={'Email'}>
          {getFieldDecorator('email', {
            rules: [
              {
                required: true,
                message: 'Please input your email!',
              },
              { validator: validator.validateEmail },
            ],
          })(
            <Input placeholder="Enter your email"/>,
          )}
        </Form.Item>
        <Button
          style={{ textTransform: 'uppercase' }}
          type="primary" block htmlType="submit"
          className="forgot_password-form-button">
          SEND THE RESET PASSWORD LINK
        </Button>
      </Form>
    )
  }

  const WrappedCustomForgotPasswordForm = Form.create({ name: 'forgot_password' })(CustomForgotPasswordForm)

  return (
    <React.Fragment>
      <Helmet>
        <title>Forgot password | Website</title>
      </Helmet>
      <Heading>Forgot password</Heading>
      <Message>We'll send an email to...</Message>
      <WrappedCustomForgotPasswordForm/>
      <NaviLink to={'/login'} className={'color-link'}>
        Back to login
      </NaviLink>
    </React.Fragment>
  )
}

export default withRouter(inject('authStore')(observer(ForgotPasswordForm)))
