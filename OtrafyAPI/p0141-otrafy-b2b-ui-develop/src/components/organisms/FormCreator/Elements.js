import React from 'react'
import {
  ElemWrapper, Button, ButtonGroup, Element,
} from './ElementsStyled'
import { Popconfirm, Icon } from 'antd'
import { inject, observer } from 'mobx-react'
// Icons
import deleteIcon from '../../../assets/icons/trash-icn@2x.png'
import editIcon from '../../../assets/icons/pencil-icn@2x.png'

const Elements = ({ type, elemIndex, formsStore, commonStore }) => {

  const deleteBtnStyle = { background: 'linear-gradient(328.13deg, #FF0844 15.19%, #F44336 87.08%)' }
  const editBtnStyle = { background: 'linear-gradient(149.04deg, #2ECF94 8.75%, #3DBEA3 86.67%)' }

  const deleteHandler = () => {
    formsStore.deleteElementFromPage(elemIndex)
  }

  const editHandler = () => {
    formsStore.showElementProperties(elemIndex)
  }

  return (
    <ElemWrapper theme={commonStore.appTheme}>
      <label>{type.title}</label>
      {
        type.description === '' ? null :
          <p>{type.description}</p>
      }
      <Element className={type.className}>
        {type.placeHolder}
        <div className="img">
          <img src={type.icon.component} alt="" width={type.icon.width} height={type.icon.height}/>
        </div>
      </Element>
      <ButtonGroup>
        <Popconfirm
          placement={'topRight'}
          okText={'Delete'} okType={'danger'} onConfirm={deleteHandler}
          title={'Are you sure you want to delete this input?'}
          icon={<Icon type="question-circle-o" style={{ color: 'red' }}/>}>
          <Button
            style={deleteBtnStyle}>
            <img src={deleteIcon} alt="Delete"/>
          </Button>
        </Popconfirm>
        <Button
          onClick={editHandler}
          style={editBtnStyle}>
          <img src={editIcon} alt="Edit"/>
        </Button>
      </ButtonGroup>
    </ElemWrapper>
  )
}

export default inject('formsStore', 'commonStore')(observer(Elements))