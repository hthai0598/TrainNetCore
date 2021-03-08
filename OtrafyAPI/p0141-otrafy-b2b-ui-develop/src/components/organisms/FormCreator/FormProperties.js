import React, { Fragment, useState } from 'react'
import { inject, observer } from 'mobx-react'
import { Input, InputNumber, Checkbox, Select, message, Form, Switch } from 'antd'
import {
  FormPropertiesWrapper, Heading, HeadingWraper,
} from './FormPropertiesStyled'
import validator from '../../../validator'
import uuid from 'uuid'

const { TextArea } = Input
const { Option } = Select

const FormProperties = ({ formsStore, commonStore }) => {

  const currentPage = formsStore.currentPageForm - 1
  const selectedElementIndex = formsStore.selectedElementIndex
  const elemInfo = formsStore.submittedFormBuilder.pages[currentPage].elements

  const handleChange = (target, value) => {
    formsStore.updateElementProperties(target, value)
  }

  const PropertiesForm = props => {

    const [tagInput, setTagInput] = useState('')

    const { getFieldDecorator } = props.form

    return (
      <Form className={'properties-form'} id={'properties-form'}>
        <Form.Item label={'Field label'}>
          {getFieldDecorator('title', {
            initialValue: elemInfo[selectedElementIndex].title,
            rules: [
              {
                required: true,
                message: `Do not leave this field blank`,
              },
              { validator: validator.validateEmptyString },
            ],
          })(
            <Input placeholder={'Field label'} onChange={e => handleChange('title', e.target.value)}/>,
          )}
        </Form.Item>
        <Form.Item label={'Instruction'}>
          {getFieldDecorator('placeHolder', {
            initialValue: elemInfo[selectedElementIndex].placeHolder,
            rules: [
              { validator: validator.validateEmptyString },
            ],
          })(
            <TextArea onChange={e => handleChange('placeHolder', e.target.value)}
                      placeholder={'Write something...'} rows={4}/>,
          )}
        </Form.Item>
        <Form.Item label={'Description'}>
          {getFieldDecorator('description', {
            initialValue: elemInfo[selectedElementIndex].description,
            rules: [
              { validator: validator.validateEmptyString },
            ],
          })(
            <Input onChange={e => handleChange('description', e.target.value)} placeholder={'Input description'}/>,
          )}
        </Form.Item>
        {
          elemInfo[selectedElementIndex].className !== 'singleLine'
          && elemInfo[selectedElementIndex].className !== 'multiLine'
          && elemInfo[selectedElementIndex].className !== 'name'
          && elemInfo[selectedElementIndex].className !== 'address'
            ? null :
            <Fragment>
              <Form.Item label={'Min character'}>
                {getFieldDecorator('minLength', {
                  initialValue: elemInfo[selectedElementIndex].minLength,
                })(
                  <InputNumber
                    onChange={e => handleChange('minLength', e)}
                    size={'large'} className={'custom-input-number'}
                    min={0} placeholder={'Input min character'}/>,
                )}
              </Form.Item>
              <Form.Item label={'Max character'}>
                {getFieldDecorator('maxLength', {
                  initialValue: elemInfo[selectedElementIndex].maxLength,
                })(
                  <InputNumber
                    onChange={e => handleChange('maxLength', e)}
                    size={'large'} className={'custom-input-number'}
                    min={0} placeholder={'Input max character'}/>,
                )}
              </Form.Item>
            </Fragment>
        }
        {
          elemInfo[selectedElementIndex].className !== 'number'
          && elemInfo[selectedElementIndex].className !== 'decimal'
            ? null :
            <Fragment>
              <Form.Item label={'Min value'}>
                {getFieldDecorator('minValue', {
                  initialValue: elemInfo[selectedElementIndex].minValue,
                })(
                  <InputNumber
                    onChange={e => handleChange('minValue', e)}
                    size={'large'} className={'custom-input-number'}
                    min={0} placeholder={'Input min value'}/>,
                )}
              </Form.Item>
              <Form.Item label={'Max value'}>
                {getFieldDecorator('maxValue', {
                  initialValue: elemInfo[selectedElementIndex].maxValue,
                })(
                  <InputNumber
                    onChange={e => handleChange('maxValue', e)}
                    size={'large'} className={'custom-input-number'}
                    min={0} placeholder={'Input max value'}/>,
                )}
              </Form.Item>
            </Fragment>
        }
        {
          elemInfo[selectedElementIndex].className !== 'date'
          && elemInfo[selectedElementIndex].className !== 'dateTime'
            ? null :
            <Fragment>
              <Form.Item label={'Date format'}>
                {getFieldDecorator('dateFormat', {
                  initialValue: elemInfo[selectedElementIndex].dateFormat,
                })(
                  <Select onChange={e => handleChange('dateFormat', e)}>
                    <Option value={'DD/MM/YYYY'}>dd/mm/yyyy</Option>
                    <Option value={'MM/DD/YYYY'}>mm/dd/yyyy</Option>
                  </Select>,
                )}
              </Form.Item>
            </Fragment>
        }
        {
          elemInfo[selectedElementIndex].className !== 'time'
          && elemInfo[selectedElementIndex].className !== 'dateTime'
            ? null :
            <Fragment>
              <Form.Item label={'Time format'}>
                {getFieldDecorator('timeFormat', {
                  initialValue: elemInfo[selectedElementIndex].timeFormat,
                })(
                  <Select onChange={e => handleChange('timeFormat', e)}>
                    <Option value={'12 hours'}>12 hours</Option>
                    <Option value={'24 hours'}>24 hours</Option>
                  </Select>,
                )}
              </Form.Item>
            </Fragment>
        }
        {
          elemInfo[selectedElementIndex].className !== 'dropdown'
          && elemInfo[selectedElementIndex].className !== 'radioButton'
          && elemInfo[selectedElementIndex].className !== 'multipleChoice'
            ? null :
            <Fragment>
              <Form.Item label={'Choices'}>
                {getFieldDecorator('choices', {
                  initialValue: elemInfo[selectedElementIndex].choices,
                })(
                  <Select
                    onSearch={val => setTagInput(val)}
                    mode={'tags'} size={'large'}
                    placeholder={'Add or remove choices'}
                    onChange={e => {
                      handleChange('choices', e)
                      setTagInput('')
                    }}
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
                    {elemInfo[selectedElementIndex].choices.map(choice =>
                      <Option key={uuid()} value={choice}>{choice}</Option>,
                    )}
                  </Select>,
                )}
              </Form.Item>
            </Fragment>
        }
        {
          elemInfo[selectedElementIndex].className !== 'radioButton'
          && elemInfo[selectedElementIndex].className !== 'multipleChoice'
            ? null :
            <Fragment>
              <Form.Item label={'Direction'}>
                {getFieldDecorator('direction', {
                  initialValue: elemInfo[selectedElementIndex].direction,
                })(
                  <Select
                    onChange={e => handleChange('direction', e)}
                    placeholder={'Display by row or col'}>
                    <Option value={'horizontal'}>Horizontal</Option>
                    <Option value={'vertical'}>Vertical</Option>
                  </Select>,
                )}
              </Form.Item>
            </Fragment>
        }
        {
          elemInfo[selectedElementIndex].className !== 'checkbox'
            ? null :
            <Fragment>
              <Form.Item label={'Confirm label'}>
                {getFieldDecorator('confirmLabel', {
                  initialValue: elemInfo[selectedElementIndex].confirmLabel,
                  rules: [
                    {
                      required: true,
                      message: 'This field can not be blank',
                    },
                    { validator: validator.validateEmptyString },
                  ],
                })(
                  <Input onChange={e => handleChange('confirmLabel', e.target.value)}
                         placeholder={'Input confirm label'}/>,
                )}
              </Form.Item>
            </Fragment>
        }
        {
          elemInfo[selectedElementIndex].className !== 'fileUpload'
          && elemInfo[selectedElementIndex].className !== 'imageUpload'
            ? null :
            <Fragment>
              <Form.Item label={'File type allowed'}>
                {getFieldDecorator('allowFileType', {
                  initialValue: elemInfo[formsStore.selectedElementIndex].allowFileType,
                })(
                  <Select
                    mode={'multiple'} size={'large'} showArrow={true}
                    placeholder={'Select allowed file type'}
                    onChange={e => handleChange('allowFileType', e)}>
                    {elemInfo[formsStore.selectedElementIndex].fileType.map(type =>
                      <Option key={uuid()} value={type}>{type}</Option>,
                    )}
                  </Select>,
                )}
              </Form.Item>
              <Form.Item label={'Max size (in MB)'}>
                {getFieldDecorator('maxSize', {
                  initialValue: elemInfo[formsStore.selectedElementIndex].maxSize,
                })(
                  <InputNumber
                    className={'custom-input-number'} size={'large'} min={0}
                    onChange={e => handleChange('maxSize', e)}
                    placeholder={'Max file size?'}/>,
                )}
              </Form.Item>
              <Form.Item>
                {getFieldDecorator('allowMultiple', {
                  valuePropName: 'checked',
                  initialValue: elemInfo[formsStore.selectedElementIndex].allowMultiple,
                })(
                  <Checkbox
                    onChange={e => handleChange('allowMultiple', e.target.checked)}>
                    Allow multiple upload?
                  </Checkbox>,
                )}
              </Form.Item>
            </Fragment>
        }
      </Form>
    )
  }

  const WrappedPropertiesForm = Form.create({ name: 'properties-form' })(PropertiesForm)

  return (
    <Fragment>
      {
        formsStore.selectedElementIndex === undefined ? null :
          <FormPropertiesWrapper>
            <HeadingWraper>
              <Heading>Properties</Heading>
              <Checkbox
                onChange={e => handleChange('isRequired', e.target.checked)}
                checked={elemInfo[formsStore.selectedElementIndex].isRequired}>
                Required
              </Checkbox>
            </HeadingWraper>
            <WrappedPropertiesForm/>
          </FormPropertiesWrapper>
      }
    </Fragment>
  )
}

export default inject('formsStore', 'commonStore')(observer(FormProperties))