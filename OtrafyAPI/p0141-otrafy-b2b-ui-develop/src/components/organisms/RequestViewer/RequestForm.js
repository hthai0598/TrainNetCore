import React from 'react'
import {
  Form, Button,
  Input, InputNumber, Radio,
  DatePicker, TimePicker, Select, Checkbox,
} from 'antd'
import { inject, observer } from 'mobx-react'
import { toJS } from 'mobx'
import { withRouter } from 'react-router-dom'
import uuid from 'uuid'
import validator from '../../../validator'
import {
  FormNavigation,
} from './RequestViewerStyled'
import moment from 'moment'
import SignatureCanvas from 'react-signature-canvas'
import CustomSignaturePad from './CustomSignaturePad'

const { TextArea } = Input
const { Option } = Select

const RequestForm = ({ formsRequestsStore, match }) => {

  const { currentPage, currentForm, surveyResult } = formsRequestsStore.survey
  const currentFormContent = formsRequestsStore.survey.surveyData[currentForm]
  const { surveyDesigner, formId } = currentFormContent
  const totalPage = JSON.parse(surveyDesigner)[0].pages.length
  const formData = JSON.parse(surveyDesigner)[0].pages[currentPage]
  const currentSurveyResult = surveyResult.length > 0 ? surveyResult[currentForm] : {}

  console.log('form data', formData)

  const conventionNamingObjKey = key => key.toLowerCase().replace(/-/, '').replace(/ /g, '_')

  const CustomForm = props => {
    const { getFieldDecorator } = props.form
    const handleSubmit = e => {
      e.preventDefault()
      props.form.validateFields((err, values) => {
        if (!err) {
          console.log(toJS(formsRequestsStore.survey.surveyResult))
        }
      })
    }
    const handleBackPage = () => {
      props.form.validateFields((err, values) => {
        if (!err) {
          formsRequestsStore.goToPrevRequestPage()
        }
      })
    }
    const handleNextPage = () => {
      props.form.validateFields((err, values) => {
        if (!err) {
          formsRequestsStore.goToNextRequestPage()
        }
      })
    }
    const handleUpdate = (key, val) => {
      formsRequestsStore.changeSurveyResult(key, val)
    }
    return (
      <Form className={formId} id={formId} onSubmit={e => handleSubmit(e)}>
        {
          formData.elements.map(elem =>
            <Form.Item key={elem.name} label={elem.title}>
              {
                elem.className !== 'name'
                && elem.className !== 'address'
                && elem.className !== 'singleLine'
                  ? null :
                  getFieldDecorator(conventionNamingObjKey(elem.title), {
                    initialValue: currentSurveyResult
                      ? currentSurveyResult[conventionNamingObjKey(elem.title)]
                      : null,
                    rules: [
                      {
                        required: elem.isRequired,
                        message: `Please input ${elem.title}`,
                      },
                      { validator: validator.validateEmptyString },
                    ],
                  })(
                    <Input
                      minLength={elem.minLength} maxLength={elem.maxLength === 0 ? 524288 : elem.maxLength}
                      onChange={e => handleUpdate(conventionNamingObjKey(elem.title), e.target.value)}
                      placeholder={elem.placeHolder}/>,
                  )
              }
              {
                elem.className !== 'phone'
                  ? null :
                  getFieldDecorator(conventionNamingObjKey(elem.title), {
                    initialValue: currentSurveyResult
                      ? currentSurveyResult[conventionNamingObjKey(elem.title)]
                      : null,
                    rules: [
                      {
                        required: elem.isRequired,
                        message: `Please input ${elem.title}`,
                      },
                      { validator: validator.validatePhoneNumber },
                    ],
                  })(
                    <Input
                      onChange={e => handleUpdate(conventionNamingObjKey(elem.title), e.target.value)}
                      placeholder={elem.placeHolder}/>,
                  )
              }
              {
                elem.className !== 'email'
                  ? null :
                  getFieldDecorator(conventionNamingObjKey(elem.title), {
                    initialValue: currentSurveyResult
                      ? currentSurveyResult[conventionNamingObjKey(elem.title)]
                      : null,
                    rules: [
                      {
                        required: elem.isRequired,
                        message: `Please input ${elem.title}`,
                      },
                      { validator: validator.validateEmail },
                    ],
                  })(
                    <Input
                      onChange={e => handleUpdate(conventionNamingObjKey(elem.title), e.target.value)}
                      placeholder={elem.placeHolder}/>,
                  )
              }
              {
                elem.className !== 'radioButton'
                  ? null :
                  getFieldDecorator(conventionNamingObjKey(elem.title), {
                    initialValue: currentSurveyResult
                      ? currentSurveyResult[conventionNamingObjKey(elem.title)]
                      : null,
                    rules: [
                      {
                        required: elem.isRequired,
                        message: `Please select ${elem.title}`,
                      },
                    ],
                  })(
                    <Radio.Group onChange={e => handleUpdate(conventionNamingObjKey(elem.title), e.target.value)}>
                      {elem.choices.map((choice, index) =>
                        <Radio
                          style={elem.direction === 'vertical'
                            ? {
                              display: 'block',
                              marginTop: 10,
                            }
                            : null}
                          key={index} value={choice}>
                          {choice}
                        </Radio>,
                      )}
                    </Radio.Group>,
                  )
              }
              {
                elem.className !== 'number'
                  ? null :
                  getFieldDecorator(conventionNamingObjKey(elem.title), {
                    initialValue: currentSurveyResult
                      ? currentSurveyResult[conventionNamingObjKey(elem.title)]
                      : null,
                    rules: [
                      {
                        required: elem.isRequired,
                        message: `Please input ${elem.title}`,
                      },
                      { validator: validator.validateIntergerNumber },
                    ],
                  })(
                    <InputNumber
                      min={elem.minValue} max={elem.maxValue === 0 ? 2147483647 : elem.maxValue}
                      size={'large'} className={'custom-input-number'}
                      placeholder={elem.placeHolder}
                      onChange={e => handleUpdate(conventionNamingObjKey(elem.title), e)}
                    />,
                  )
              }
              {
                elem.className !== 'decimal'
                  ? null :
                  getFieldDecorator(conventionNamingObjKey(elem.title), {
                    initialValue: currentSurveyResult
                      ? currentSurveyResult[conventionNamingObjKey(elem.title)]
                      : null,
                    rules: [
                      {
                        required: elem.isRequired,
                        message: `Please input ${elem.title}`,
                      },
                    ],
                  })(
                    <InputNumber
                      min={elem.minValue} max={elem.maxValue === 0 ? 2147483647 : elem.maxValue}
                      size={'large'} className={'custom-input-number'}
                      placeholder={elem.placeHolder}
                      onChange={e => handleUpdate(conventionNamingObjKey(elem.title), e)}
                    />,
                  )
              }
              {
                elem.className !== 'multiLine'
                  ? null :
                  getFieldDecorator(conventionNamingObjKey(elem.title), {
                    initialValue: currentSurveyResult
                      ? currentSurveyResult[conventionNamingObjKey(elem.title)]
                      : null,
                    rules: [
                      {
                        required: elem.isRequired,
                        message: `Please input ${elem.title}`,
                      },
                      { validator: validator.validateEmptyString },
                    ],
                  })(
                    <TextArea
                      minLength={elem.minLength} maxLength={elem.maxLength === 0 ? 524288 : elem.maxLength}
                      onChange={e => handleUpdate(conventionNamingObjKey(elem.title), e.target.value)}
                      placeholder={elem.placeHolder}/>,
                  )
              }
              {
                elem.className !== 'time'
                  ? null :
                  getFieldDecorator(conventionNamingObjKey(elem.title), {
                    initialValue: currentSurveyResult
                      ? currentSurveyResult[conventionNamingObjKey(elem.title)]
                        ? moment(currentSurveyResult[conventionNamingObjKey(elem.title)], 'hh:mm:ss a')
                        : null
                      : null,
                    rules: [
                      {
                        required: elem.isRequired,
                        message: `Please select ${elem.title}`,
                      },
                    ],
                  })(
                    <TimePicker
                      use12Hours={elem.timeFormat === '12 hours'}
                      onChange={(time, timeString) => handleUpdate(conventionNamingObjKey(elem.title), timeString)}
                      placeholder={elem.placeholder}/>,
                  )
              }
              {
                elem.className !== 'date'
                  ? null :
                  getFieldDecorator(conventionNamingObjKey(elem.title), {
                    initialValue: currentSurveyResult
                      ? currentSurveyResult[conventionNamingObjKey(elem.title)]
                        ? moment(currentSurveyResult[conventionNamingObjKey(elem.title)], elem.dateFormat)
                        : null
                      : null,
                    rules: [
                      {
                        required: elem.isRequired,
                        message: `Please input ${elem.title}`,
                      },
                    ],
                  })(
                    <DatePicker
                      onChange={(date, dateString) => handleUpdate(conventionNamingObjKey(elem.title), dateString)}
                      format={elem.dateFormat}
                      placeholder={elem.placeHolder}/>,
                  )
              }
              {
                elem.className !== 'dateTime'
                  ? null :
                  getFieldDecorator(conventionNamingObjKey(elem.title), {
                    initialValue: currentSurveyResult
                      ? currentSurveyResult[conventionNamingObjKey(elem.title)]
                        ? moment(currentSurveyResult[conventionNamingObjKey(elem.title)], `${elem.dateFormat} ${elem.timeFormat === '12 hours' ? 'hh:mm:ss a' : 'HH:mm:ss'}`)
                        : null
                      : null,
                    rules: [
                      {
                        required: elem.isRequired,
                        message: `Please input ${elem.title}`,
                      },
                    ],
                  })(
                    <DatePicker
                      onChange={(date, dateString) => handleUpdate(conventionNamingObjKey(elem.title), dateString)}
                      format={`${elem.dateFormat} ${elem.timeFormat === '12 hours' ? 'hh:mm:ss a' : 'HH:mm:ss'}`}
                      showTime placeholder={elem.placeHolder}/>,
                  )
              }
              {
                elem.className !== 'dropdown'
                  ? null :
                  getFieldDecorator(conventionNamingObjKey(elem.title), {
                    initialValue: currentSurveyResult
                      ? currentSurveyResult[conventionNamingObjKey(elem.title)]
                      : null,
                    rules: [
                      {
                        required: elem.isRequired,
                        message: `Please select ${elem.title}`,
                      },
                    ],
                  })(
                    <Select
                      onChange={val => handleUpdate(conventionNamingObjKey(elem.title), val)}
                      placeholder={elem.placeHolder}>
                      {elem.choices.map((choice, index) =>
                        <Option key={index} value={choice}>
                          {choice}
                        </Option>,
                      )}
                    </Select>,
                  )
              }
              {
                elem.className !== 'multipleChoice'
                  ? null :
                  getFieldDecorator(conventionNamingObjKey(elem.title), {
                    initialValue: currentSurveyResult
                      ? currentSurveyResult[conventionNamingObjKey(elem.title)]
                      : null,
                    rules: [
                      {
                        required: elem.isRequired,
                        message: `Please select ${elem.title}`,
                      },
                    ],
                  })(
                    <Checkbox.Group
                      onChange={choice => handleUpdate(conventionNamingObjKey(elem.title), choice)}
                      options={elem.choices}/>,
                  )
              }
              {
                elem.className !== 'checkbox'
                  ? null :
                  getFieldDecorator(conventionNamingObjKey(elem.title), {
                    valuePropName: 'checked',
                    initialValue: currentSurveyResult
                      ? currentSurveyResult[conventionNamingObjKey(elem.title)]
                      : null,
                    rules: [
                      {
                        required: elem.isRequired,
                        message: `Please confirm ${elem.title}`,
                      },
                    ],
                  })(
                    <Checkbox onChange={e => handleUpdate(conventionNamingObjKey(elem.title), e.target.checked)}>
                      {elem.confirmLabel}
                    </Checkbox>,
                  )
              }
              {
                elem.className !== 'signaturePad'
                  ? null :
                  <CustomSignaturePad
                    onSave={signature => handleUpdate(conventionNamingObjKey(elem.title), signature)}
                    data={
                      currentSurveyResult
                        ? <img src={currentSurveyResult[conventionNamingObjKey(elem.title)]} alt=""/>
                        : null
                    }/>
              }
            </Form.Item>,
          )
        }
        <FormNavigation style={{ display: totalPage === 1 ? 'none' : 'flex' }}>
          {
            currentPage === 0
              ? null :
              <Button
                type={'link'}
                onClick={handleBackPage}>
                Previous page
              </Button>
          }
          <span className={'total'}>Page {currentPage + 1}/{totalPage}</span>
          {
            currentPage + 1 === totalPage
              ? null :
              <Button
                type={'link'}
                onClick={handleNextPage}>
                Next page
              </Button>
          }
        </FormNavigation>
      </Form>
    )
  }

  const WrappedCustomForm = Form.create({ name: uuid() })(CustomForm)

  return <WrappedCustomForm/>
}

export default withRouter(inject('formsRequestsStore')(observer(RequestForm)))