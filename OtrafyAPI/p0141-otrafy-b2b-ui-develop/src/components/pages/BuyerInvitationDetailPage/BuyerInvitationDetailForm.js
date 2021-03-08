import React, { useEffect } from 'react'
import { withRouter } from 'react-router-dom'
import { Form, Input, Button, Row, Col, Select } from 'antd'
import validator from '../../../validator'
import { inject, observer } from 'mobx-react'
import FormButtonGroup from '../../organisms/FormButtonGroup'

const { TextArea } = Input
const { Option } = Select

const BuyerInvitationDetailForm = ({ history, location, match, companiesStore }) => {

  const buyerId = match.params.buyerId

  const permission = () => {
    let temp = companiesStore.buyerInvitationDetail.permission
    if (temp) {
      return temp.map(p => {
        switch (p) {
          case 0:
            return 'Run report'
          case 1:
            return 'View all supplier'
          case 2:
            return 'Create form template'
          case 3:
            return 'Create new supplier'
        }
      })
    }
  }

  useEffect(() => {
    companiesStore.getBuyerDetail(buyerId)
  }, [])

  useEffect(() => {
  }, [companiesStore.buyerInvitationDetail])

  const CustomBuyerInvitationDetailForm = props => {

    const { getFieldDecorator } = props.form

    const handleSubmit = e => {
      e.preventDefault()
      props.form.validateFields((err, values) => {
        if (!err) {
          let convertedPermission = () => values.permission.map(p => {
            switch (p) {
              case 'Run report':
                return 0
              case 'View all supplier':
                return 1
              case 'Create form template':
                return 2
              case 'Create new supplier':
                return 3
            }
          })
          let submitValues = {
            ...values,
            permission: convertedPermission(),
          }
          companiesStore.updateBuyerDetail(buyerId, submitValues)
        }
      })
    }

    return (
      <Form className={'update-buyer-form'} onSubmit={e => handleSubmit(e)}>
        <Form.Item label={'Email'}>
          {getFieldDecorator('email', {
            initialValue: companiesStore.buyerInvitationDetail ? companiesStore.buyerInvitationDetail.email : null,
            rules: [
              {
                required: true,
                message: 'Please input buyer email!',
              },
              { validator: validator.validateEmail },
            ],
          })(
            <Input placeholder="Buyer email"/>,
          )}
        </Form.Item>
        <Row type={'flex'} justify={'space-between'}>
          <Col xs={24} md={11}>
            <Form.Item label={'First name'}>
              {getFieldDecorator('firstName', {
                initialValue: companiesStore.buyerInvitationDetail ? companiesStore.buyerInvitationDetail.firstName : null,
                rules: [
                  {
                    required: true,
                    message: 'Please input buyer first name!',
                  },
                  { validator: validator.validateEmptyString },
                ],
              })(
                <Input placeholder="Buyer first name"/>,
              )}
            </Form.Item>
          </Col>
          <Col xs={24} md={11}>
            <Form.Item label={'Last name'}>
              {getFieldDecorator('lastName', {
                initialValue: companiesStore.buyerInvitationDetail ? companiesStore.buyerInvitationDetail.lastName : null,
                rules: [
                  {
                    required: true,
                    message: 'Please input buyer last name!',
                  },
                  { validator: validator.validateEmptyString },
                ],
              })(
                <Input placeholder="Buyer last name"/>,
              )}
            </Form.Item>
          </Col>
        </Row>
        <Form.Item label={'Username'}>
          {getFieldDecorator('username', {
            initialValue: companiesStore.buyerInvitationDetail ? companiesStore.buyerInvitationDetail.username : null,
            rules: [
              {
                required: true,
                message: 'Please input buyer username!',
              },
              { validator: validator.validateEmptyString },
            ],
          })(
            <Input placeholder={'Input username'}/>,
          )}
        </Form.Item>
        <Form.Item label={'Permission'}>
          {getFieldDecorator('permission', {
            initialValue: companiesStore.buyerInvitationDetail ? permission() : null,
          })(
            <Select
              showArrow={true}
              size={'large'}
              mode={'multiple'}>
              <Option value={'Run report'}>Run report</Option>
              <Option value={'View all supplier'}>View all supplier</Option>
              <Option value={'Create form template'}>Create form template</Option>
              <Option value={'Create new supplier'}>Create new supplier</Option>
            </Select>,
          )}
        </Form.Item>
        <Form.Item label={'Job title'}>
          {getFieldDecorator('jobTitle', {
            initialValue: companiesStore.buyerInvitationDetail ? companiesStore.buyerInvitationDetail.jobTitle : null,
            rules: [
              { validator: validator.validateEmptyString },
            ],
          })(
            <Input placeholder="Buyer job title"/>,
          )}
        </Form.Item>
        <FormButtonGroup>
          <Button type={'danger'} ghost onClick={() => history.goBack()}>
            Go back
          </Button>
          <Button type={'primary'} htmlType={'submit'}>
            Save changes
          </Button>
        </FormButtonGroup>
      </Form>
    )

  }

  const WrappedCustomBuyerInvitationDetailForm = Form.create({ name: 'update-buyer' })(CustomBuyerInvitationDetailForm)

  return <WrappedCustomBuyerInvitationDetailForm/>

}

export default withRouter(inject('companiesStore')(observer(BuyerInvitationDetailForm)))