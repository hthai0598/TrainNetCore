import React, { useState } from 'react'
import { Form, Input, Button } from 'antd'
import { inject, observer } from 'mobx-react'
import { withRouter } from 'react-router-dom'
import {
  Heading, Message, NaviLink,
} from './CustomStyled'

const CreateBuyerFromInvitationForm = ({ buyersStore, history, match }) => {

  const inviteTokenId = match.params.invitedId

  const CustomForm = props => {

    const { getFieldDecorator } = props.form

    const [confirmDirty, setConfirm] = useState(false)

    const handleSubmit = (e) => {
      e.preventDefault()
      props.form.validateFields((err, values) => {
        if (!err) {
          let submitValue = {
            tokenId: inviteTokenId,
            newPassword: values.password,
          }
          buyersStore.activeBuyer(submitValue, history)
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
      <Form className={'create-buyer-password-form'} onSubmit={(e) => handleSubmit(e)}>
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
            <Input type="password" placeholder="Enter your password"/>,
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
            <Input type="password" placeholder="Re-type your password" onBlur={handleConfirmBlur}/>,
          )}
        </Form.Item>
        <Button block type={'primary'}
                style={{ textTransform: 'uppercase' }}
                htmlType="submit">
          CREATE ACCOUNT
        </Button>
      </Form>
    )

  }

  const WrappedCustomForm = Form.create({ name: 'create-buyer-from-invitation' })(CustomForm)

  return (
    <React.Fragment>
      <Heading>Create password</Heading>
      <Message>Enter your password</Message>
      <WrappedCustomForm/>
      <NaviLink to={'/login'} className={'color-link'}>
        Already have an account? Back to login
      </NaviLink>
    </React.Fragment>
  )

}

export default withRouter(inject('buyersStore')(observer(CreateBuyerFromInvitationForm)))