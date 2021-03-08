import React from 'react'
import { Form, Input, Button, Select } from 'antd'
import validator from '../../../validator'
import FormButtonGroup from '../../organisms/FormButtonGroup'
import { withRouter } from 'react-router-dom'
import { inject, observer } from 'mobx-react'

const { Option } = Select
const { TextArea } = Input

const CreateNewRequestForm = props => {

  const {
    history, suppliersList, productsList, formsList,
    formsRequestsStore,
  } = props

  const CustomCreateNewRequestForm = props => {

    const { getFieldDecorator } = props.form

    const handleSubmit = (e) => {
      e.preventDefault()
      props.form.validateFields((err, values) => {
        if (!err) {
          formsRequestsStore.createRequest(values, history)
        }
      })
    }

    const handleClearForm = () => {
      props.form.setFields({
        form: {
          value: [],
        },
      })
    }

    const handleSelectSupplier = id => {
      formsRequestsStore.updateFormRequestData('selectedSupplierId', id)
    }

    const updateFormRequestData = (key, val) => {
      formsRequestsStore.updateFormRequestData(key, val)
    }

    return (
      <Form className={'add-supplier-form'} id={'add-supplier-form'} onSubmit={e => handleSubmit(e)}>
        <Form.Item label={'Request title'}>
          {getFieldDecorator('title', {
            initialValue: formsRequestsStore.formRequestData.title,
            rules: [
              {
                required: true,
                message: `Please input request title!`,
              },
              { validator: validator.validateEmptyString },
            ],
          })(
            <Input onChange={e => updateFormRequestData('title', e.target.value)} placeholder={'Enter request title'}/>,
          )}
        </Form.Item>
        <Form.Item label={'Description'}>
          {getFieldDecorator('description', {
            initialValue: formsRequestsStore.formRequestData.description,
            rules: [
              { validator: validator.validateEmptyString },
            ],
          })(
            <TextArea
              onChange={e => updateFormRequestData('description', e.target.value)}
              rows={2} placeholder={'Input request description'}/>,
          )}
        </Form.Item>
        <Form.Item label={'Supplier name'}>
          {getFieldDecorator('supplierId', {
            initialValue: formsRequestsStore.formRequestData.selectedSupplierId,
            rules: [
              {
                required: true,
                message: `Please select supplier name!`,
              },
            ],
          })(
            <Select
              optionFilterProp={'filter'}
              onChange={id => handleSelectSupplier(id)}
              showSearch={true}
              placeholder={'Choose suppliers name'}>
              {suppliersList.map(supplier =>
                <Option key={supplier.id} filter={supplier.name} value={supplier.id}>
                  {supplier.name}
                </Option>)}
            </Select>,
          )}
        </Form.Item>
        <Form.Item label={'Product'}>
          {getFieldDecorator('productId', {
            initialValue: formsRequestsStore.formRequestData.selectedProductId,
            rules: [
              {
                required: true,
                message: `Please select supplier products!`,
              },
            ],
          })(
            <Select
              optionFilterProp={'filter'} showSearch={true}
              placeholder={'Select supplier’s products'}>
              {productsList.map(product =>
                <Option key={product.id} value={product.id}>
                  {product.name}
                </Option>)}
            </Select>,
          )}
        </Form.Item>
        <div className={'form-row-with-action'}>
          <div className="input">
            <Form.Item label={'Form'} style={{ marginBottom: 0 }}>
              {getFieldDecorator('form', {
                rules: [
                  {
                    required: true,
                    message: `Please select form!`,
                  },
                ],
              })(
                <Select
                  optionFilterProp={'filter'}
                  mode={'multiple'} size={'large'}
                  showArrow={true}
                  placeholder={'Select form'}
                  style={{ width: '100%' }}>
                  {formsList.map(form =>
                    <Option key={form.id} filter={form.name} value={form.id}>
                      {form.name}
                    </Option>)}
                </Select>,
              )}
            </Form.Item>
          </div>
          <div className="action">
            <Button type={'danger'} ghost onClick={handleClearForm}>
              Clear all
            </Button>
          </div>
        </div>
        <Form.Item label={'Message to supplier:'}>
          {getFieldDecorator('message')(
            <TextArea rows={5} placeholder={'Write something...'}/>,
          )}
        </Form.Item>
        <FormButtonGroup>
          <Button type={'danger'} ghost onClick={() => history.push(`/suppliers-management`)}>
            Cancel
          </Button>
          <Button type={'primary'} htmlType={'submit'}>
            Send request
          </Button>
        </FormButtonGroup>
      </Form>
    )

  }

  const WrappedCustomCreateNewRequestForm = Form.create({ name: 'add-request' })(CustomCreateNewRequestForm)

  return <WrappedCustomCreateNewRequestForm/>

}

export default withRouter(inject('suppliersStore', 'formsRequestsStore')(observer(CreateNewRequestForm)))