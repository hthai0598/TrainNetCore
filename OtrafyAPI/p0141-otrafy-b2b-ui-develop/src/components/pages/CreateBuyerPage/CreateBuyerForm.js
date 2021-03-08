import React from 'react'
import { Form, Input, Button, Select, Row, Col } from 'antd'
import validator from '../../../validator'
import { inject, observer } from 'mobx-react'
import { withRouter } from 'react-router-dom'
import FormButtonGroup from '../../organisms/FormButtonGroup'

const { Option } = Select
const { TextArea } = Input

const CreateBuyerForm = ({ buyersStore, history, match }) => {

  const CustomCreateBuyerForm = props => {

    const { getFieldDecorator } = props.form

    const handleSubmit = (e) => {
      e.preventDefault()
      props.form.validateFields((err, values) => {
        if (!err) {
          let submitValues = {
            companyId: match.params.companyId,
            firstName: values.firstName,
            lastName: values.lastName,
            username: values.username,
            permission: values.permission,
            email: values.email,
            jobTitle: values.jobTitle,
            messageToBuyer: values.messageToBuyer,
          }
          buyersStore.createBuyerInvitation(submitValues, history)
        }
      })
    }

    return (
      <Form className={'create-buyer-form'} onSubmit={e => handleSubmit(e)}>
        <Row type={'flex'} justify={'space-between'}>
          <Col xs={24} md={11}>
            <Form.Item label={'First name'}>
              {getFieldDecorator('firstName', {
                rules: [
                  {
                    required: true,
                    message: `Please input first name!`,
                  },
                  { validator: validator.validateEmptyString },
                ],
              })(
                <Input placeholder="First name"/>,
              )}
            </Form.Item>
          </Col>
          <Col xs={24} md={11}>
            <Form.Item label={'Last name'}>
              {getFieldDecorator('lastName', {
                rules: [
                  {
                    required: true,
                    message: `Please input last name!`,
                  },
                  { validator: validator.validateEmptyString },
                ],
              })(
                <Input placeholder="Last name"/>,
              )}
            </Form.Item>
          </Col>
        </Row>
        <Form.Item label={'Username'}>
          {getFieldDecorator('username', {
            rules: [
              {
                required: true,
                message: `Please input buyer username!`,
              },
              { validator: validator.validateEmptyString },
            ],
          })(
            <Input placeholder="Username"/>,
          )}
        </Form.Item>
        <Form.Item label={'Email'}>
          {getFieldDecorator('email', {
            rules: [
              {
                required: true,
                message: `Please input buyer email!`,
              },
              { validator: validator.validateEmail },
            ],
          })(
            <Input placeholder="Email"/>,
          )}
        </Form.Item>
        <Form.Item label={'Permission'}>
          {getFieldDecorator('permission', {
            initialValue: [],
            rules: [
              {
                required: true,
                message: `Please select permission (allow multiple selection)`
              }
            ]
          })(
            <Select
              showArrow={true}
              placeholder={'Select role'}
              mode={'multiple'}
              size={'large'}>
              <Option value={0}>Run report</Option>
              <Option value={1}>View all supplier</Option>
              <Option value={2}>Create form template</Option>
              <Option value={3}>Create new supplier</Option>
            </Select>,
          )}
        </Form.Item>
        <Form.Item label={'Job title (optional)'}>
          {getFieldDecorator('jobTitle', {})(
            <Input placeholder="Job title"/>,
          )}
        </Form.Item>
        <Form.Item label={'Message to buyer (optional)'}>
          {getFieldDecorator('messageToBuyer', {})(
            <TextArea rows={4} placeholder="Message to buyer"/>,
          )}
        </Form.Item>
        <FormButtonGroup>
          <Button
            type={'primary'}
            htmlType="submit">
            Create new buyer
          </Button>
        </FormButtonGroup>
      </Form>
    )
  }

  const WrappedCustomCreateBuyerForm = Form.create({ name: 'create-buyer' })(CustomCreateBuyerForm)

  return <WrappedCustomCreateBuyerForm/>

}

export default withRouter(inject('buyersStore')(observer(CreateBuyerForm)))