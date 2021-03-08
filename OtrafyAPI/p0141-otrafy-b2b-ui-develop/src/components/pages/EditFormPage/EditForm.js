import React, { useState } from 'react'
import { Form, Button, Select, Input, Radio } from 'antd'
import { inject, observer } from 'mobx-react'
import { withRouter } from 'react-router-dom'
import validator from '../../../validator'
import FormButtonGroup from '../../organisms/FormButtonGroup'

const { Option } = Select

const EditForm = props => {

  const {
    tagsList,
    history,
    commonStore, buyersStore, tagsStore, formsStore,
  } = props

  const CustomForm = props => {

    const [tagInput, setTagInput] = useState('')
    const { getFieldDecorator } = props.form

    const handleSubmit = (e) => {
      e.preventDefault()
      props.form.validateFields((err) => {
        if (!err) {
          buyersStore.setFormCreationProgress(2)
          commonStore.toggleCollapseSidebar(true)
        }
      })
    }

    const onUpdate = (key, val) => {
      buyersStore.updateFormCreateValues(key, val)
    }

    const onChangeTags = val => {
      setTagInput('')
      buyersStore.updateFormCreateValues('tags', val.filter(tag => tag.trim()))
    }

    const handleCancel = () => {
      buyersStore.clearForm()
      tagsStore.clearTags()
      formsStore.clearAllFormsData()
      history.push(`/all-forms`)
    }

    return (
      <Form onSubmit={e => handleSubmit(e)}>
        <Form.Item label={'Create from'}>
          {getFieldDecorator('formType', {
            initialValue: buyersStore.formCreateValues.formType === 0 ? 'Blank form' : 'Form template',
          })(
            <Radio.Group onChange={e => onUpdate('formType', e.target.value)}>
              <Radio value="Blank form">Blank form</Radio>
              <Radio value="Form template" disabled>Form template</Radio>
            </Radio.Group>,
          )}
        </Form.Item>
        <Form.Item label={'Form name'}>
          {getFieldDecorator('name', {
            initialValue: buyersStore.formCreateValues.name,
            rules: [
              {
                required: true,
                message: `Please input form name!`,
              },
              { validator: validator.validateEmptyString },
            ],
          })(
            <Input onChange={e => onUpdate('name', e.target.value)} placeholder="Enter form name"/>,
          )}
        </Form.Item>
        <Form.Item label={'Tags'}>
          {getFieldDecorator('tags', {
            initialValue: buyersStore.formCreateValues.tags,
          })(
            <Select
              onSearch={val => setTagInput(val)}
              onChange={val => onChangeTags(val)}
              showArrow={true} mode={'tags'} size={'large'}
              placeholder={'Tags'}
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
        <Form.Item label={'Description'}>
          {getFieldDecorator('description', {
            initialValue: buyersStore.formCreateValues.description,
            rules: [
              { validator: validator.validateEmptyString },
            ],
          })(
            <Input.TextArea rows={5} onChange={e => onUpdate('description', e.target.value)}
                            placeholder="Write something..."/>,
          )}
        </Form.Item>
        <FormButtonGroup>
          <Button
            type={'danger'} ghost
            onClick={handleCancel}>
            Cancel
          </Button>
          <Button type={'primary'} htmlType={'submit'}>
            Next step
          </Button>
        </FormButtonGroup>
      </Form>
    )

  }

  const WrappedCustomForm = Form.create({ name: 'update-form-detail' })(CustomForm)

  return <WrappedCustomForm/>
}

export default withRouter(inject('buyersStore', 'commonStore', 'tagsStore', 'formsStore')(observer(EditForm)))