import React, { useState } from 'react'
import { withRouter } from 'react-router-dom'
import { inject, observer } from 'mobx-react'
import { Form, Button, Input, Select, InputNumber } from 'antd'
import validator from '../../../validator'
import FormButtonGroup from '../../organisms/FormButtonGroup'

const { TextArea } = Input
const { Option } = Select

const AddProductForm = ({ productsStore, tagsStore, commonStore, tagsList, suppliersStore, history, match }) => {

  const supplierId = match.params.supplierId

  const CustomForm = props => {

    const [tagInput, setTagInput] = useState('')

    const { getFieldDecorator } = props.form

    const handleSubmit = (e) => {
      e.preventDefault()
      props.form.validateFields((err, values) => {
        if (!err) {
          let submitValues = {
            ...values,
            tags: values.tags.filter(el => el.trim()),
            grade: values.grade !== null ? values.grade : undefined,
            supplierId: supplierId,
          }
          tagsStore.checkAndCreateNewTag(submitValues.tags)
          productsStore.createProduct(submitValues, history)
        }
      })
    }

    const handleCancelAddProduct = () => {
      suppliersStore.setActiveTab('3')
      history.push(`/suppliers-management/suppliers-detail/${supplierId}`)
    }

    return (
      <Form className={'add-product-form'} id={'add-product-form'} onSubmit={e => handleSubmit(e)}>
        <Form.Item label={'Product name'}>
          {getFieldDecorator('name', {
            rules: [
              {
                required: true,
                message: `Please input product name!`,
              },
              { validator: validator.validateEmptyString },
            ],
          })(
            <Input placeholder={'Input product name'}/>,
          )}
        </Form.Item>
        <Form.Item label={'Product ID'}>
          {getFieldDecorator('code', {
            rules: [
              {
                required: true,
                message: `Please input product ID!`,
              },
              { validator: validator.validateEmptyString },
            ],
          })(
            <Input placeholder={'Input product ID'}/>,
          )}
        </Form.Item>
        <Form.Item label={'Tags'}>
          {getFieldDecorator('tags')(
            <Select
              onSearch={val => setTagInput(val)}
              onChange={() => setTagInput('')}
              mode={'tags'} size={'large'} showArrow={true}
              placeholder={`Type to search tags or add new tag`}
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
              {tagsList.map(tag => <Option key={tag}>{tag}</Option>)}
            </Select>,
          )}
        </Form.Item>
        <Form.Item label={'Product grade'}>
          {getFieldDecorator('grade')(
            <InputNumber className={'custom-input-number'} size={'large'}
                         min={1} max={100} placeholder={'Input product grade'}/>,
          )}
        </Form.Item>
        <Form.Item label={'Description'}>
          {getFieldDecorator('description')(
            <TextArea rows={4} placeholder={'Write product description here'}/>,
          )}
        </Form.Item>
        <FormButtonGroup>
          <Button type={'danger'} ghost
                  onClick={handleCancelAddProduct}>
            Cancel
          </Button>
          <Button type={'primary'} htmlType={'submit'}>
            Create product
          </Button>
        </FormButtonGroup>
      </Form>
    )
  }

  const WrappedCustomForm = Form.create({ name: 'add-product' })(CustomForm)

  return <WrappedCustomForm/>
}

export default withRouter(inject('productsStore', 'tagsStore', 'suppliersStore', 'commonStore')(observer(AddProductForm)))