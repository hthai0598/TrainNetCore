import React, { useState } from 'react'
import { Form, Input, Button } from 'antd'
import {
  Heading,
  Message,
  NaviLink,
} from './CustomStyled'


const ResetPasswordForm = ({ onReset }) => {

  const CustomResetPasswordForm = props => {

    const { getFieldDecorator } = props.form

    const [confirmDirty, setConfirm] = useState(false)

    const handleSubmit = (e) => {
      e.preventDefault()
      props.form.validateFields((err, values) => {
        if (!err) {
          onReset(values.password)
        }
      })
    }

    const compareToFirstPassword = (rule, value, callback) => {
      if (value && value !== props.form.getFieldValue('password')) {
        callback('Password not match!')
      } else {
        callback()
      }
    }

    const validateToNextPassword = (rule, value, callback) => {
      if (value && confirmDirty) {
        props.form.validateFields(['confirm'], { force: true })
      }
      callback()
    }

    const handleConfirmBlur = (e) => {
      const value = e.target.value
      setConfirm(confirmDirty || !!value)
    }

    return (
      <Form className={'reset-password-form'} onSubmit={(e) => handleSubmit(e)}>
        <Form.Item label={'Password'}>
          {getFieldDecorator('password', {
            rules: [
              {
                required: true,
                message: 'Please input your password!',
              },
              { validator: validateToNextPassword },
            ],
          })(
            <Input type="password" placeholder="Enter your new password"/>,
          )}
        </Form.Item>
        <Form.Item label={'Confirm password'}>
          {getFieldDecorator('confirm', {
            rules: [
              {
                required: true,
                message: 'Please re-type your password!',
              },
              { validator: compareToFirstPassword },
            ],
          })(
            <Input type="password" placeholder="Re-type new password" onBlur={handleConfirmBlur}/>,
          )}
        </Form.Item>
        <Button
          block type={'primary'} htmlType="submit"
          style={{ marginBottom: 15, textTransform: 'uppercase' }}
          className="login-form-button">
          CONFIRM
        </Button>
      </Form>
    )
  }

  const WrappedCustomResetPasswordForm = Form.create({ name: 'reset-password' })(CustomResetPasswordForm)

  return (
    <React.Fragment>
      <Heading>Reset password</Heading>
      <Message>Enter your new password</Message>
      <WrappedCustomResetPasswordForm/>
      <NaviLink to={'/login'} className={'color-link'}>
        Back to login
      </NaviLink>
    </React.Fragment>
  )
}

export default ResetPasswordForm