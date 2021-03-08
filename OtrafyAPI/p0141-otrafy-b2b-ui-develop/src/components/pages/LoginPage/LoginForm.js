import React, { useEffect } from 'react'
import { Form, Input, Button, Checkbox } from 'antd'
import validator from '../../../validator'
import { inject, observer } from 'mobx-react'
import { withRouter } from 'react-router-dom'
import {
  Heading,
  Message,
  NaviLink,
} from './CustomStyled'

const LoginForm = ({ authStore, history }) => {

  const CustomLoginForm = props => {

    const { getFieldDecorator } = props.form

    const handleSubmit = (e) => {
      e.preventDefault()
      props.form.validateFields((err, values) => {
        if (!err) {
          authStore.userLogin(values.username, values.password, values.remember, history)
        }
      })
    }

    return (
      <Form className={'login-form'} onSubmit={(e) => handleSubmit(e)}>
        <Form.Item label={'Email'}>
          {getFieldDecorator('username', {
            rules: [
              {
                required: true,
                message: 'Please input your email/username!',
              },
              { validator: validator.validateEmptyString },
            ],
          })(
            <Input placeholder="Login with email or username"/>,
          )}
        </Form.Item>
        <Form.Item label={'Password'}>
          {getFieldDecorator('password', {
            rules: [
              {
                required: true,
                message: 'Please input your password!',
              },
            ],
          })(
            <Input type="password" placeholder="Password"/>,
          )}
        </Form.Item>
        <Form.Item className={'remember'}>
          {getFieldDecorator('remember', {
            valuePropName: 'checked',
            initialValue: true,
          })(<Checkbox>Remember me</Checkbox>)}
          <NaviLink to="/forgot-password" className={'color-link'}>
            Forgot password
          </NaviLink>
        </Form.Item>
        <Button block type={'primary'}
                style={{
                  marginBottom: 15,
                  textTransform: 'uppercase',
                }}
                htmlType="submit">
          Log in
        </Button>
      </Form>
    )
  }
  const WrappedCustomLoginForm = Form.create({ name: 'login' })(CustomLoginForm)

  return (
    <React.Fragment>
      <Heading>Welcome back!</Heading>
      <Message>Sign in by entering the information below.</Message>
      <WrappedCustomLoginForm/>
    </React.Fragment>
  )
}

export default withRouter(inject('authStore')(observer(LoginForm)))
