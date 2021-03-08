import React, { useEffect } from 'react'
import { Form, Button, Input, InputNumber, Switch } from 'antd'
import validator from '../../../validator'
import { CompanyDetailCardEditWrapper } from './CustomStyled'

const CompanyDetailCardEdit = ({ companyData, onSubmit, onCancel }) => {

  const UpdateCompanyForm = props => {

    useEffect(() => {
      props.form.validateFields()
    }, [])

    const { getFieldDecorator } = props.form

    const handleSubmit = e => {
      e.preventDefault()
      props.form.validateFields((err, values) => {
        if (!err) {
          onSubmit(companyData.companyId, values)
        }
      })
    }

    return (
      <Form className={'update-company-form'} onSubmit={e => handleSubmit(e)}>
        <Form.Item label={'Company name'}>
          {getFieldDecorator('name', {
            initialValue: companyData.name,
            rules: [
              {
                required: true,
                message: 'Please input company name!',
              },
              { validator: validator.validateEmptyString },
            ],
          })(
            <Input placeholder="Company name"/>,
          )}
        </Form.Item>
        <Form.Item label={'Company address'}>
          {getFieldDecorator('address', {
            initialValue: companyData.address,
            rules: [
              {
                required: true,
                message: 'Please input company address!',
              },
              { validator: validator.validateEmptyString },
            ],
          })(
            <Input placeholder="Company address"/>,
          )}
        </Form.Item>
        <Form.Item label={'Company email'}>
          {getFieldDecorator('email', {
            initialValue: companyData.email,
            rules: [
              {
                required: true,
                message: 'Please input company email!',
              },
              { validator: validator.validateEmail },
            ],
          })(
            <Input type="email" placeholder="Company email"/>,
          )}
        </Form.Item>
        <Form.Item label={'Company phone number'}>
          {getFieldDecorator('phone', {
            initialValue: companyData.phone,
            rules: [
              {
                required: true,
                message: 'Please input company phone number!',
              },
              { validator: validator.validatePhoneNumber },
            ],
          })(
            <Input placeholder="Company phone number"/>,
          )}
        </Form.Item>
        <Form.Item label={'Company website'}>
          {getFieldDecorator('website', {
            initialValue: companyData.website,
            rules: [
              {
                required: true,
                message: 'Please input company website!',
              },
              { validator: validator.validateWebsite },
            ],
          })(
            <Input placeholder="Company website"/>,
          )}
        </Form.Item>
        <Form.Item label={'Maximum number of buyers allowed'}>
          {getFieldDecorator('maxNumberBuyersAllowed', {
            initialValue: companyData.maxNumberBuyersAllowed,
            rules: [
              {
                required: true,
                message: 'Please input allowed maximum number of buyers!',
              },
              { validator: validator.validateIntergerNumber },
            ],
          })(
            <InputNumber size={'large'} min={0} max={2147483647}
                         className={'custom-input-number'}
                         placeholder="Maximum number of buyers allowed"
                         style={{ width: '100%' }}/>,
          )}
        </Form.Item>
        <Form.Item label={'Maximum number of suppliers allowed'}>
          {getFieldDecorator('maxNumberSuppliersAllowed', {
            initialValue: companyData.maxNumberSuppliersAllowed,
            rules: [
              {
                required: true,
                message: 'Please input allowed maximum number of suppliers!',
              },
              { validator: validator.validateIntergerNumber },
            ],
          })(
            <InputNumber size={'large'} min={0} max={2147483647}
                         className={'custom-input-number'}
                         placeholder="Maximum number of suppliers allowed"
                         style={{ width: '100%' }}/>,
          )}
        </Form.Item>
        <Form.Item label={'Maximum number of forms allowed'}>
          {getFieldDecorator('maxNumberFormsAllowed', {
            initialValue: companyData.maxNumberFormsAllowed,
            rules: [
              {
                required: true,
                message: 'Please input allowed maximum number of forms!',
              },
              { validator: validator.validateIntergerNumber },
            ],
          })(
            <InputNumber size={'large'} min={0} max={2147483647}
                         className={'custom-input-number'}
                         placeholder="Maximum number of forms allowed"
                         style={{ width: '100%' }}/>,
          )}
        </Form.Item>
        <Form.Item label={'Activate this company'}>
          {getFieldDecorator('isActive', {
            valuePropName: 'checked',
            initialValue: companyData.isActive,
          })(
            <Switch/>,
          )}
        </Form.Item>
        <Button
          block
          type={'primary'}
          style={{
            height: 40,
            lineHeight: '40px',
            marginBottom: 15,
          }}
          htmlType="submit">
          Save changes
        </Button>
        <Button
          block
          style={{
            height: 40,
            lineHeight: '40px',
          }}
          onClick={onCancel}>
          Cancel
        </Button>
      </Form>
    )
  }

  const WrappedUpdateCompanyForm = Form.create({ name: 'update-company' })(UpdateCompanyForm)

  return (
    <CompanyDetailCardEditWrapper>
      <WrappedUpdateCompanyForm/>
    </CompanyDetailCardEditWrapper>
  )
}

export default CompanyDetailCardEdit