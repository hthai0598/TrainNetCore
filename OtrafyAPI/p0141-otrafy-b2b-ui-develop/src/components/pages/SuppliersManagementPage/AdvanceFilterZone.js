import React from 'react'
import closeIcon from '../../../assets/icons/x-mark@2x.png'
import {
  FilterZoneWrapper,
  CloseFilterButton, FilterFormWrapper,
} from './CustomStyled'
import { Button, Form, message, Select } from 'antd'
import { inject, observer } from 'mobx-react'

const { Option } = Select

const AdvanceFilterZone = props => {

  const {
    formsList,
    onClose,
    suppliersStore,
  } = props

  const CustomFilterForm = props => {

    const { getFieldDecorator } = props.form

    const handleSubmit = (e) => {
      e.preventDefault()
      props.form.validateFields((err, values) => {
        if (!err) {
          if (Object.values(values).every(el => el === undefined)) {
            message.error(`Please select at least one filter type`)
          } else {
            console.log(values)
          }
        }
      })
    }

    return (
      <Form className={'filter-suppliers-form'} onSubmit={e => handleSubmit(e)}>
        <FilterFormWrapper>
          <div className="form-col">
            <Form.Item label={'Status'}>
              {getFieldDecorator('status')(
                <Select placeholder={'Select status'}>
                  <Option value="Pending">Pending</Option>
                  <Option value="In Progress">In Progress</Option>
                  <Option value="Completed">Completed</Option>
                  <Option value="Approved">Approved</Option>
                  <Option value="Rejected">Rejected</Option>
                </Select>,
              )}
            </Form.Item>
          </div>
          <div className="form-col">
            <Form.Item label={'Last buyer in contact:'}>
              {getFieldDecorator('lastBuyers')(
                <Select placeholder={'Select last buyer'}>
                  <Option value="Completed">Completed</Option>
                  <Option value="In Progress">In Progress</Option>
                  <Option value="Rejected">Rejected</Option>
                  <Option value="Reviewal">Reviewal</Option>
                  <Option value="Unseen">Unseen</Option>
                </Select>,
              )}
            </Form.Item>
          </div>
          <div className="form-col">
            <Form.Item label={'Form'}>
              {getFieldDecorator('form')(
                <Select
                  optionFilterProp={'filter'}
                  showSearch={true} showArrow={true}
                  placeholder={'Select form'}>
                  {formsList.map(form =>
                    <Option key={form.id} filter={form.name} value={form.id}>
                      {form.name}
                    </Option>)}
                </Select>,
              )}
            </Form.Item>
          </div>
          <Button
            htmlType={'submit'}
            type={'primary'}
            style={{
              height: 40,
              width: 90,
            }}>
            Filter
          </Button>
        </FilterFormWrapper>
      </Form>
    )
  }

  const WrappedCustomFilterForm = Form.create({ name: 'filter-suppliers-form' })(CustomFilterForm)

  return (
    <FilterZoneWrapper>
      <CloseFilterButton onClick={onClose}>
        <img src={closeIcon} alt="Close"/>
      </CloseFilterButton>
      <WrappedCustomFilterForm/>
    </FilterZoneWrapper>
  )
}

export default inject('suppliersStore')(observer(AdvanceFilterZone))