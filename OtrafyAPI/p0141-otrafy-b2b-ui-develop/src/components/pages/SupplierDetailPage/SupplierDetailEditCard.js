import React, { useEffect, useState } from 'react'
import { Form, Input, Button, Select, Row, Col } from 'antd'
import { toJS } from 'mobx'
import { withRouter } from 'react-router-dom'
import { inject, observer } from 'mobx-react'
import validator from '../../../validator'
import suppliersStore from '../../../stores/suppliersStore'
import { FormWrapper, FormButtonGroup } from './CustomStyled'

const { Option } = Select
const { TextArea } = Input

const SupplierDetailEditCard = ({ suppliersStore, commonStore, tagsStore, tagsList, match }) => {

  const supplierId = match.params.supplierId
  const supplierDetail = toJS(suppliersStore.supplierDetail)

  useEffect(() => {
    suppliersStore.getSupplierDetail(supplierId)
  }, [])

  const CustomOverviewForm = props => {

    const [tagInput, setTagInput] = useState('')

    const { getFieldDecorator } = props.form

    const handleSubmit = e => {
      e.preventDefault()
      props.form.validateFields((err, values) => {
        if (!err) {
          if (suppliersStore.supplierDetailActiveTab === '1') {
            tagsStore.checkAndCreateNewTag(values.tags)
          }
          suppliersStore.updateSupplierDetail(supplierId, values)
        }
      })
    }

    return (
      <Form className={'update-supplier-form'} onSubmit={e => handleSubmit(e)}>
        <Form.Item label={'Company name'}>
          {getFieldDecorator('companyName', {
            initialValue: supplierDetail.companyProfiles ? supplierDetail.companyProfiles.companyName : null,
            rules: [
              {
                required: true,
                message: 'Please input company name!',
              },
              { validator: validator.validateEmptyString },
            ],
          })(
            <Input placeholder={'Company name'}/>,
          )}
        </Form.Item>
        <Form.Item label={'Email'}>
          {getFieldDecorator('email', {
            initialValue: supplierDetail.email,
            rules: [
              {
                required: true,
                message: 'Please input company email!',
              },
              { validator: validator.validateEmail },
            ],
          })(
            <Input placeholder={'Company email'}/>,
          )}
        </Form.Item>
        {
          suppliersStore.supplierDetailActiveTab === '1' ? null :
            <React.Fragment>
              <Form.Item label={'Phone number'}>
                {getFieldDecorator('phone', {
                  initialValue: supplierDetail.userProfiles ? supplierDetail.userProfiles.phone : null,
                  rules: [
                    { validator: validator.validatePhoneNumber },
                  ],
                })(
                  <Input placeholder={'Input phone number'}/>,
                )}
              </Form.Item>
              <Row type={'flex'} justify={'space-between'}>
                <Col xs={24} md={11}>
                  <Form.Item label={'First name'}>
                    {getFieldDecorator('firstName', {
                      initialValue: supplierDetail.userProfiles ? supplierDetail.userProfiles.firstName : null,
                      rules: [
                        {
                          required: true,
                          message: 'Please input first name!',
                        },
                        { validator: validator.validateEmptyString },
                      ],
                    })(
                      <Input placeholder={'Input first name'}/>,
                    )}
                  </Form.Item>
                </Col>
                <Col xs={24} md={11}>
                  <Form.Item label={'Last name'}>
                    {getFieldDecorator('lastName', {
                      initialValue: supplierDetail.userProfiles ? supplierDetail.userProfiles.lastName : null,
                      rules: [
                        {
                          required: true,
                          message: 'Please input last name!',
                        },
                        { validator: validator.validateEmptyString },
                      ],
                    })(
                      <Input placeholder={'Input last name'}/>,
                    )}
                  </Form.Item>
                </Col>
              </Row>
              <Form.Item label={'Job title'}>
                {getFieldDecorator('jobTitle', {
                  initialValue: supplierDetail.userProfiles ? supplierDetail.userProfiles.jobTitle : null,
                  rules: [
                    {
                      validator: validator.validateEmptyString,
                    },
                  ],
                })(
                  <Input placeholder={'Input job title'}/>,
                )}
              </Form.Item>
              <Form.Item label={'Address'}>
                {getFieldDecorator('address', {
                  initialValue: supplierDetail.companyProfiles ? supplierDetail.companyProfiles.address : null,
                  rules: [
                    { validator: validator.validateEmptyString },
                  ],
                })(
                  <Input placeholder={'Input address'}/>,
                )}
              </Form.Item>
            </React.Fragment>
        }
        {
          suppliersStore.supplierDetailActiveTab === '2' ? null :
            <Form.Item label={'Tags'}>
              {getFieldDecorator('tags', {
                initialValue: supplierDetail.tags,
              })(
                <Select
                  onSearch={val => setTagInput(val)}
                  onChange={() => setTagInput('')}
                  showArrow={true} placeholder={'Select tags'} mode={'tags'} size={'large'}
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
        }
        <Form.Item label={'Description'}>
          {getFieldDecorator('description', {
            initialValue: supplierDetail.companyProfiles ? supplierDetail.companyProfiles.description : null,
          })(
            <TextArea rows={4} placeholder={'Input description'}/>,
          )}
        </Form.Item>
        <FormButtonGroup>
          <Button type={'danger'} ghost onClick={() => suppliersStore.toggleEditMode(false)}>
            Cancel
          </Button>
          <Button type={'primary'} htmlType={'submit'}>
            Save changes
          </Button>
        </FormButtonGroup>
      </Form>
    )

  }
  const WrappedCustomOverviewForm = Form.create({ name: 'update-supplier' })(CustomOverviewForm)

  return (
    <FormWrapper>
      <div className="heading">Update supplier info:</div>
      <WrappedCustomOverviewForm/>
    </FormWrapper>
  )
}

export default withRouter(inject('suppliersStore', 'commonStore', 'tagsStore')(observer(SupplierDetailEditCard)))