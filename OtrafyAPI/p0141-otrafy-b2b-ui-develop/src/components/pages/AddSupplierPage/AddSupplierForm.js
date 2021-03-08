import React, { useState } from 'react'
import { Form, Input, Button, Select, Row, Col } from 'antd'
import validator from '../../../validator'
import FormButtonGroup from '../../organisms/FormButtonGroup'
import { inject, observer } from 'mobx-react'
import { toJS } from 'mobx'
import { withRouter } from 'react-router-dom'

const { Option } = Select
const { TextArea } = Input

const AddSupplierForm = ({ suppliersStore, commonStore, tagsStore, tagsList, selectedTags, history }) => {

  const CustomAddSupplierForm = props => {

    const [tagInput, setTagInput] = useState('')

    const { getFieldDecorator } = props.form

    const handleSubmit = (e) => {
      e.preventDefault()
      props.form.validateFields((err, values) => {
        if (!err) {
          let submitValues = {
            ...values,
            tags: toJS(values.tags).filter(el => el.trim()),
            username: values.email,
            isSendEmailInvitation: true,
          }
          tagsStore.checkAndCreateNewTag(toJS(values.tags).filter(el => el.trim()))
          suppliersStore.createSupplier(submitValues, history)
        }
      })
    }

    const onUpdate = (key, val) => {
      suppliersStore.updateAddSupplierFormValues(key, val)
    }

    return (
      <Form className={'add-supplier-form'} id={'add-supplier-form'} onSubmit={e => handleSubmit(e)}>
        <Form.Item label={'Company'}>
          {getFieldDecorator('companyName', {
            initialValue: suppliersStore.addSupplierFormValues.companyName,
            rules: [
              {
                required: true,
                message: `Please input company name!`,
              },
              { validator: validator.validateEmptyString },
            ],
          })(
            <Input onChange={e => onUpdate('companyName', e.target.value)} placeholder="Enter company name"/>,
          )}
        </Form.Item>
        <Form.Item label={'Tags'}>
          {getFieldDecorator('tags')(
            <Select
              onSearch={val => setTagInput(val)}
              onChange={() => setTagInput('')}
              showArrow={true}
              mode={'tags'} size={'large'}
              placeholder={'Type to search tags or add new tag'}
              dropdownRender={menu => (
                <div>
                  {menu}
                  {
                    !tagInput
                      ? null
                      : <a style={{
                        padding: 15,
                        fontWeight: 500,
                        wordBreak: 'break-all',
                        color: commonStore.appTheme.solidColor,
                        display: 'block',
                      }}>
                        Add new tag "{tagInput}"
                      </a>
                  }
                </div>
              )}>
              {tagsList.map(tag => <Option key={tag} value={tag}>{tag}</Option>)}
            </Select>,
          )}
        </Form.Item>
        <Form.Item label={'Email'}>
          {getFieldDecorator('email', {
            initialValue: suppliersStore.addSupplierFormValues.email,
            rules: [
              {
                required: true,
                message: `Please input suppliers email!`,
              },
              { validator: validator.validateEmail },
            ],
          })(
            <Input onChange={e => onUpdate('email', e.target.value)} placeholder="Enter email"/>,
          )}
        </Form.Item>
        <Row type={'flex'} justify={'space-between'}>
          <Col xs={24} md={11}>
            <Form.Item label={'First name'}>
              {getFieldDecorator('firstName', {
                initialValue: suppliersStore.addSupplierFormValues.firstName,
                rules: [
                  {
                    required: true,
                    message: `Please input first name!`,
                  },
                  { validator: validator.validateEmptyString },
                ],
              })(
                <Input onChange={e => onUpdate('firstName', e.target.value)} placeholder="First name"/>,
              )}
            </Form.Item>
          </Col>
          <Col xs={24} md={11}>
            <Form.Item label={'Last name'}>
              {getFieldDecorator('lastName', {
                initialValue: suppliersStore.addSupplierFormValues.lastName,
                rules: [
                  {
                    required: true,
                    message: `Please input last name!`,
                  },
                  { validator: validator.validateEmptyString },
                ],
              })(
                <Input onChange={e => onUpdate('lastName', e.target.value)} placeholder="Last name"/>,
              )}
            </Form.Item>
          </Col>
        </Row>
        <Form.Item label={'Message to supplier'}>
          {getFieldDecorator('messageToSupplier', {
            rules: [
              { validator: validator.validateEmptyString },
            ],
          })(
            <TextArea rows={4} placeholder={'Message to supplier'}/>,
          )}
        </Form.Item>
        <FormButtonGroup>
          <Button type={'danger'} ghost onClick={() => history.push('/suppliers-management')}>
            Cancel
          </Button>
          <Button type="primary" htmlType="submit">
            Add supplier
          </Button>
        </FormButtonGroup>
      </Form>
    )
  }

  const WrappedCustomAddSupplierForm = Form.create({ name: 'add-supplier' })(CustomAddSupplierForm)

  return <WrappedCustomAddSupplierForm/>

}

export default withRouter(inject('suppliersStore', 'tagsStore', 'commonStore')(observer(AddSupplierForm)))