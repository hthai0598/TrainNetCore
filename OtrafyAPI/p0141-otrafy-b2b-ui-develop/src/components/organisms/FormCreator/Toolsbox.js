import React from 'react'
import {
  Wrapper, List, FieldsTitle,
} from './ToolsboxStyled'
// Icons
import singleLine from '../../../assets/form-elems/singleLine@2x.png'
import multiLine from '../../../assets/form-elems/multiLine@2x.png'
import number from '../../../assets/form-elems/number@2x.png'
import decimal from '../../../assets/form-elems/decimal@2x.png'
import name from '../../../assets/form-elems/name@2x.png'
import address from '../../../assets/form-elems/address@2x.png'
import phone from '../../../assets/form-elems/phone@2x.png'
import email from '../../../assets/form-elems/email@2x.png'
import date from '../../../assets/form-elems/date@2x.png'
import time from '../../../assets/form-elems/time@2x.png'
import dropdown from '../../../assets/form-elems/dropdown@2x.png'
import radioButton from '../../../assets/form-elems/radioButton@2x.png'
import multipleChoice from '../../../assets/form-elems/multipleChoice@2x.png'
import checkbox from '../../../assets/form-elems/checkbox@2x.png'
import dateTime from '../../../assets/form-elems/dateTime@2x.png'
import website from '../../../assets/form-elems/website@2x.png'
import currency from '../../../assets/form-elems/currency@2x.png'
import fileUpload from '../../../assets/form-elems/fileUpload@2x.png'
import imageUpload from '../../../assets/form-elems/imageUpload@2x.png'
import section from '../../../assets/form-elems/section@2x.png'
import pageBreak from '../../../assets/form-elems/pageBreak@2x.png'
import signaturePad from '../../../assets/form-elems/signature@2x.png'

const Toolsbox = ({ onDrag }) => {

  const basicFields = [
    {
      title: 'Single line',
      className: 'singleLine',
      icon: {
        component: singleLine,
        width: 10,
        height: 4,
      },
      isRequired: false,
      description: '',
      placeHolder: 'Write something...',
      // Unique field
      minLength: 0,
      maxLength: 0,
    },
    {
      title: 'Multi line',
      className: 'multiLine',
      icon: {
        component: multiLine,
        width: 11,
        height: 7,
      },
      isRequired: false,
      description: '',
      placeHolder: 'Write something...',
      // Unique field
      minLength: 0,
      maxLength: 0,
    },
    {
      title: 'Number',
      className: 'number',
      icon: {
        component: number,
        width: 14,
        height: 10,
      },
      isRequired: false,
      description: '',
      placeHolder: 'Select number',
      // Unique field
      minValue: 0,
      maxValue: 0,
    },
    {
      title: 'Decimal',
      className: 'decimal',
      icon: {
        component: decimal,
        width: 14,
        height: 14,
      },
      isRequired: false,
      description: '',
      placeHolder: 'Select number',
      // Unique field
      minValue: 0,
      maxValue: 0,
    },
    {
      title: 'Name',
      className: 'name',
      icon: {
        component: name,
        width: 10,
        height: 10,
      },
      isRequired: false,
      description: '',
      placeHolder: 'Input name',
      // Unique field
      minLength: 0,
      maxLength: 0,
    },
    {
      title: 'Address',
      className: 'address',
      icon: {
        component: address,
        width: 11,
        height: 11.5,
      },
      isRequired: false,
      description: '',
      placeHolder: 'Input address',
      // Unique field
      minLength: 0,
      maxLength: 0,
    },
    {
      title: 'Phone',
      className: 'phone',
      icon: {
        component: phone,
        width: 11,
        height: 11,
      },
      isRequired: false,
      description: '',
      placeHolder: 'Input phone number',
      // Unique field
    },
    {
      title: 'Email',
      className: 'email',
      icon: {
        component: email,
        width: 12,
        height: 10,
      },
      isRequired: false,
      description: '',
      placeHolder: 'Input email',
      // Unique field
    },
    {
      title: 'Date',
      className: 'date',
      icon: {
        component: date,
        width: 11,
        height: 12,
      },
      isRequired: false,
      description: '',
      placeHolder: 'Input date',
      // Unique field
      dateFormat: 'DD/MM/YYYY',
    },
    {
      title: 'Time',
      className: 'time',
      icon: {
        component: time,
        width: 12,
        height: 12,
      },
      isRequired: false,
      description: '',
      placeHolder: 'Input time',
      // Unique field
      timeFormat: '12 hours',
    },
    {
      title: 'Dropdown',
      className: 'dropdown',
      icon: {
        component: dropdown,
        width: 8,
        height: 4,
      },
      isRequired: false,
      description: '',
      placeHolder: 'Select item',
      // Unique field
      choices: ['item1', 'item2', 'item3'],
    },
    {
      title: 'Radio button',
      className: 'radioButton',
      icon: {
        component: radioButton,
        width: 14,
        height: 14,
      },
      isRequired: false,
      description: '',
      placeHolder: 'Select item',
      // Unique field
      direction: 'horizontal',
      choices: ['item1', 'item2', 'item3'],
    },
    {
      title: 'Multiple choice',
      className: 'multipleChoice',
      icon: {
        component: multipleChoice,
        width: 11.5,
        height: 9,
      },
      isRequired: false,
      description: '',
      placeHolder: 'Select multiple item',
      // Unique field
      direction: 'horizontal',
      choices: ['item1', 'item2', 'item3'],
    },
    {
      title: 'Checkbox',
      className: 'checkbox',
      icon: {
        component: checkbox,
        width: 11,
        height: 11,
      },
      isRequired: false,
      description: '',
      placeHolder: 'Select item',
      // Unique field
      confirmLabel: 'Confirm label',
    },
    {
      title: 'Date - Time',
      className: 'dateTime',
      icon: {
        component: dateTime,
        width: 14,
        height: 14,
      },
      isRequired: false,
      description: '',
      placeHolder: 'Input date-time',
      // Unique field
      timeFormat: '12 hours',
      dateFormat: 'DD/MM/YYYY',
    },
    {
      title: 'Website',
      className: 'website',
      icon: {
        component: website,
        width: 12,
        height: 12,
      },
      isRequired: false,
      description: '',
      placeHolder: 'Input website',
      // Unique field
    },
    {
      title: 'Currency',
      className: 'currency',
      icon: {
        component: currency,
        width: 6.5,
        height: 11,
      },
      isRequired: false,
      description: '',
      placeHolder: 'Input currency',
      // Unique field
      prefix: 'VND',
    },
    {
      title: 'File upload',
      className: 'fileUpload',
      icon: {
        component: fileUpload,
        width: 14,
        height: 10,
      },
      isRequired: false,
      description: '',
      placeHolder: 'Choose file to upload',
      // Unique field
      allowMultiple: false,
      maxSize: 0,
      fileType: [
        'doc', 'xls', 'ppt', 'pdf',
      ],
      allowFileType: [],
    },
    {
      title: 'Image upload',
      className: 'imageUpload',
      icon: {
        component: imageUpload,
        width: 12.5,
        height: 12.5,
      },
      isRequired: false,
      description: '',
      placeHolder: 'Choose image to upload',
      // Unique field
      allowMultiple: false,
      maxSize: 0,
      fileType: [
        'png', 'jpg', 'bmp', 'gif',
      ],
      allowFileType: [],
    },
    {
      title: 'Signature pad',
      className: 'signaturePad',
      icon: {
        component: signaturePad,
        width: 14,
        height: 12,
      },
      isRequired: false,
      description: '',
      placeHolder: 'Input your signature here',
    },
    // Special elements
    {
      title: 'Section',
      className: 'section',
      icon: {
        component: section,
        width: 11,
        height: 9,
      },
      description: '',
      placeHolder: '',
      // Unique field
    },
    {
      title: 'Page break',
      className: 'pageBreak',
      icon: {
        component: pageBreak,
        width: 14,
        height: 14,
      },
    },
  ]


  return (
    <Wrapper>
      <FieldsTitle>Basic fields</FieldsTitle>
      <List>
        {
          basicFields.map(elem => {
            return (
              <li key={elem.title} draggable
                  onDragStart={() => onDrag(elem)}>
                <div className="img">
                  <img src={elem.icon.component} alt="" width={elem.icon.width} height={elem.icon.height}/>
                </div>
                {elem.title}
              </li>
            )
          })
        }
      </List>
    </Wrapper>
  )
}

export default Toolsbox