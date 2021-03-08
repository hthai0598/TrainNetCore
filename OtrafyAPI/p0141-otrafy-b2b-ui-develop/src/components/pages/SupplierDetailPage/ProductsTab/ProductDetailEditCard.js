import React, { useState } from 'react'
import { inject, observer } from 'mobx-react'
import { toJS } from 'mobx'
import { Form, Input, InputNumber, Button, Select } from 'antd'
import validator from '../../../../validator'
import { FormButtonGroup } from '../CustomStyled'
import { FormWrapper } from './ProductsTabStyled'

const { TextArea } = Input
const { Option } = Select

const ProductDetailEditCard = ({ productsStore, tagsStore, commonStore, tagsList }) => {

  const CustomForm = props => {

    const [tagInput, setTagInput] = useState('')

    const { getFieldDecorator } = props.form

    const handleSubmit = (e) => {
      e.preventDefault()
      props.form.validateFields((err, values) => {
        if (!err) {
          let submitValues = {
            ...values,
            tags: toJS(values.tags),
          }
          tagsStore.checkAndCreateNewTag(toJS(values.tags))
          productsStore.updateProductDetail(productsStore.productInfo.id, submitValues)
        }
      })
    }

    return (
      <Form className={'edit-product-form'} id={'edit-product-form'} onSubmit={e => handleSubmit(e)}>
        <Form.Item label={'Product name'}>
          {getFieldDecorator('name', {
            initialValue: productsStore.productInfo.name,
            rules: [
              {
                required: true,
                message: 'Please input product name',
              },
              { validator: validator.validateEmptyString },
            ],
          })(
            <Input placeholder={'Input product name'}/>,
          )}
        </Form.Item>
        <Form.Item label={'Product ID'}>
          {getFieldDecorator('code', {
            initialValue: productsStore.productInfo.code,
            rules: [
              {
                required: true,
                message: 'Please input product ID',
              },
              { validator: validator.validateEmptyString },
            ],
          })(
            <Input placeholder={'Input product ID'}/>,
          )}
        </Form.Item>
        <Form.Item label={'Product grade'}>
          {getFieldDecorator('grade', {
            initialValue: productsStore.productInfo.grade,
          })(
            <InputNumber className={'custom-input-number'} size={'large'}
                         min={1} max={100} placeholder={'Input product grade'}/>,
          )}
        </Form.Item>
        <Form.Item label={'Tags'}>
          {getFieldDecorator('tags', {
            initialValue: productsStore.productInfo.tags,
          })(
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
              {
                tagsList.map(tag => <Option key={tag} value={tag}>{tag}</Option>)
              }
            </Select>,
          )}
        </Form.Item>
        <Form.Item label={'Product description'}>
          {getFieldDecorator('description', {
            initialValue: productsStore.productInfo.description,
          })(
            <TextArea rows={4} placeholder={'Write product description here'}/>,
          )}
        </Form.Item>
        <FormButtonGroup>
          <Button type={'danger'} ghost onClick={() => productsStore.toggleEditMode(false)}>
            Cancel
          </Button>
          <Button type={'primary'} htmlType={'submit'}>
            Save changes
          </Button>
        </FormButtonGroup>
      </Form>
    )

  }

  const WrappedCustomForm = Form.create({ name: 'edit-product' })(CustomForm)

  return (
    <FormWrapper>
      <div className="heading">
        Edit product detail
      </div>
      <WrappedCustomForm/>
    </FormWrapper>
  )
}

export default inject('productsStore', 'tagsStore', 'commonStore')(observer(ProductDetailEditCard))