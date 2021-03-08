import React from 'react'
import { Form, Input, Row, Col } from 'antd'
import validator from '../../../validator'
import { FormHeading } from './CustomStyled'

const { TextArea } = Input

const UserInfoForm = ({ onSubmit, info }) => {

  const CustomForm = props => {

    const { getFieldDecorator } = props.form

    const handleSubmit = (e) => {
      e.preventDefault()
      props.form.validateFields((err, values) => {
        if (!err) {
          onSubmit(values)
        }
      })
    }

    return (
      <Form className={'user-info-update-form'} id={'user-info-update-form'}
            style={{ width: '100%' }} onSubmit={e => handleSubmit(e)}>
        <FormHeading>
          Update personal information:
        </FormHeading>
        <Row type={'flex'} justify={'space-between'}>
          <Col xs={24} md={11}>
            <Form.Item label={'First name'}>
              {getFieldDecorator('firstName', {
                initialValue: info.userProfiles ? info.userProfiles.firstName : null,
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
                initialValue: info.userProfiles ? info.userProfiles.lastName : null,
                rules: [
                  {
                    required: true,
                    message: `Please input last name!`,
                  },
                  { validator: validator.validateEmptyString },
                ],
              })(
                <Input placeholder="First name"/>,
              )}
            </Form.Item>
          </Col>
        </Row>
        <Form.Item label={'Email'}>
          {getFieldDecorator('email', {
            initialValue: info.email,
            rules: [
              {
                required: true,
                message: `Please input email!`,
              },
              { validator: validator.validateEmail },
            ],
          })(
            <Input placeholder={'User email'}/>
          )}
        </Form.Item>
        <Form.Item label={'Phone number'}>
          {getFieldDecorator('phone', {
            initialValue: info.userProfiles ? info.userProfiles.phone : null,
            rules: [
              { validator: validator.validatePhoneNumber },
            ],
          })(
            <Input placeholder="Phone number"/>,
          )}
        </Form.Item>
        <Form.Item label={'Job title'}>
          {getFieldDecorator('jobTitle', {
            initialValue: info.userProfiles ? info.userProfiles.jobTitle : null,
            rules: [
              { validator: validator.validateEmptyString },
            ],
          })(
            <Input placeholder="Job title"/>,
          )}
        </Form.Item>
        <FormHeading>
          Update company information:
        </FormHeading>
        <Form.Item label={'Company name'}>
          {getFieldDecorator('companyName', {
            initialValue: info.companyProfiles ? info.companyProfiles.companyName : null,
            rules: [
              { validator: validator.validateEmptyString },
            ],
          })(
            <Input placeholder="Company name"/>,
          )}
        </Form.Item>
        <Form.Item label={'Company address'}>
          {getFieldDecorator('address', {
            initialValue: info.companyProfiles ? info.companyProfiles.address : null,
            rules: [
              { validator: validator.validateEmptyString },
            ],
          })(
            <Input placeholder="Company address"/>,
          )}
        </Form.Item>
        <Form.Item label={'Description'}>
          {getFieldDecorator('description', {
            initialValue: info.companyProfiles ? info.companyProfiles.description : null,
            rules: [
              { validator: validator.validateEmptyString },
            ],
          })(
            <TextArea rows={4} placeholder="Description"/>,
          )}
        </Form.Item>
      </Form>
    )
  }

  const WrappedUserInfoForm = Form.create({ name: 'user-info-update' })(CustomForm)

  return <WrappedUserInfoForm/>
}

export default UserInfoForm