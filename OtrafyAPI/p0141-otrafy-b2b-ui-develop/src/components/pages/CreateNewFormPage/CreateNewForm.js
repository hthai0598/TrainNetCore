import React, { useState } from 'react'
import { Form, Input, Button, Select, Radio } from 'antd'
import validator from '../../../validator'
import { inject, observer } from 'mobx-react'
import { withRouter } from 'react-router-dom'
import FormButtonGroup from '../../organisms/FormButtonGroup'

const { Option } = Select

const CreateNewForm = ({ buyersStore, tagsStore, tagsList, commonStore, history }) => {

  const CustomCreateNewForm = props => {

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

    return (
      <Form className={'create-new-form'} id={'create-new-form'} onSubmit={e => handleSubmit(e)}>
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
            rules: [
              {
                required: true,
                message: `Please select tags`,
              },
            ],
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
          <Button type={'danger'} ghost onClick={() => history.push(`/all-forms`)}>
            Cancel
          </Button>
          <Button type={'primary'} htmlType={'submit'}>
            Next step
          </Button>
        </FormButtonGroup>
      </Form>
    )

  }

  const WrappedCustomCreateNewForm = Form.create({ name: 'create-new-form' })(CustomCreateNewForm)

  return <WrappedCustomCreateNewForm/>

}

export default withRouter(inject('tagsStore', 'buyersStore', 'commonStore')(observer(CreateNewForm)))