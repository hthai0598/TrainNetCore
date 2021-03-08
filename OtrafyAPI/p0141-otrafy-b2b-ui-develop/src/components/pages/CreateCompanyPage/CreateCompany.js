import React, { useEffect } from 'react'
import { Form, Input, InputNumber, Button } from 'antd'
import validator from '../../../validator'
import FormButtonGroup from '../../organisms/FormButtonGroup'

const CreateCompany = ({ onSubmit }) => {

  const CreateCompanyForm = props => {

    useEffect(() => {
      props.form.validateFields()
    }, [])

    const { getFieldDecorator, getFieldsError, getFieldError, isFieldTouched } = props.form

    const handleSubmit = (e) => {
      e.preventDefault()
      props.form.validateFields((err, values) => {
        if (!err) {
          onSubmit(values)
        }
      })
    }

    const companyNameError = isFieldTouched('name') && getFieldError('name')
    const companyAddressError = isFieldTouched('address') && getFieldError('address')
    const companyEmailError = isFieldTouched('email') && getFieldError('email')
    const companyPhoneNumberError = isFieldTouched('phone') && getFieldError('phone')
    const companyWebsiteError = isFieldTouched('website') && getFieldError('website')
    const maxNumBuyersError = isFieldTouched('maxNumberBuyersAllowed') && getFieldError('maxNumberBuyersAllowed')
    const maxNumSuppliersError = isFieldTouched('maxNumberSuppliersAllowed') && getFieldError('maxNumberSuppliersAllowed')
    const maxNumFormsError = isFieldTouched('maxNumberFormsAllowed') && getFieldError('maxNumberFormsAllowed')

    const hasErrors = fieldsError => Object.keys(fieldsError).some(field => fieldsError[field])

    return (
      <Form className={'create-company-form'} id={'create-company-form'} onSubmit={e => handleSubmit(e)}>
        <Form.Item label={'Company name'}
                   validateStatus={companyNameError ? 'error' : ''}
                   help={companyNameError || ''}>
          {getFieldDecorator('name', {
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
        <Form.Item label={'Company address'}
                   validateStatus={companyAddressError ? 'error' : ''}
                   help={companyAddressError || ''}>
          {getFieldDecorator('address', {
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
        <Form.Item label={'Company email'}
                   validateStatus={companyEmailError ? 'error' : ''}
                   help={companyEmailError || ''}>
          {getFieldDecorator('email', {
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
        <Form.Item label={'Company phone number'}
                   validateStatus={companyPhoneNumberError ? 'error' : ''}
                   help={companyPhoneNumberError || ''}>
          {getFieldDecorator('phone', {
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
        <Form.Item label={'Company website'}
                   validateStatus={companyWebsiteError ? 'error' : ''}
                   help={companyWebsiteError || ''}>
          {getFieldDecorator('website', {
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
        <Form.Item label={'Maximum number of buyers allowed'}
                   validateStatus={maxNumBuyersError ? 'error' : ''}
                   help={maxNumBuyersError || ''}>
          {getFieldDecorator('maxNumberBuyersAllowed', {
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
        <Form.Item label={'Maximum number of suppliers allowed'}
                   validateStatus={maxNumSuppliersError ? 'error' : ''}
                   help={maxNumSuppliersError || ''}>
          {getFieldDecorator('maxNumberSuppliersAllowed', {
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
        <Form.Item label={'Maximum number of forms allowed'}
                   validateStatus={maxNumFormsError ? 'error' : ''}
                   help={maxNumFormsError || ''}>
          {getFieldDecorator('maxNumberFormsAllowed', {
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
        <FormButtonGroup>
          <Button
            type={'primary'}
            disabled={hasErrors(getFieldsError())}
            htmlType="submit"
            className="login-form-button">
            Create new company
          </Button>
        </FormButtonGroup>
      </Form>
    )
  }

  const WrappedCreateCompanyForm = Form.create({ name: 'create-company' })(CreateCompanyForm)

  return <WrappedCreateCompanyForm/>
}

export default CreateCompany