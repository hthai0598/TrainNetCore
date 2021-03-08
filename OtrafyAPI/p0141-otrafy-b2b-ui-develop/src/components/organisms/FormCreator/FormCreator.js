import React from 'react'
import {
  FormCreatorWrapper,
} from './FormCreatorStyled'
import Toolsbox from './Toolsbox'
import Editor from './Editor'
import FormProperties from './FormProperties'
import { inject, observer } from 'mobx-react'

const FormCreator = ({ formsStore }) => {

  return (
    <FormCreatorWrapper>
      <Toolsbox onDrag={item => formsStore.selectItemFromBasicFields(item)}/>
      <Editor droppedItem={formsStore.draggedItemFromBasicFields}/>
      <FormProperties/>
    </FormCreatorWrapper>
  )
}

export default inject('formsStore')(observer(FormCreator))