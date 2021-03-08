import React from 'react'
import Elements from './Elements'
import { Popconfirm, Icon } from 'antd'
import { inject, observer } from 'mobx-react'
import { withRouter } from 'react-router-dom'
import {
  Wrapper, EditorHeading, EditorMain, EmptyWrapper,
  PageIndicator, FormPageNavigation, PageFooter,
} from './EditorStyled'
import { SortableContainer, SortableElement } from 'react-sortable-hoc'
import arrayMove from 'array-move'

const Editor = ({ droppedItem, buyersStore, commonStore, formsStore }) => {

  const elementListOnPage = formsStore.submittedFormBuilder.pages[formsStore.currentPageForm - 1].elements

  const onDropItemHandler = droppedItem => {
    if (droppedItem.title !== 'Page break') {
      formsStore.addItemToFormPage(droppedItem)
    } else {
      formsStore.addPage()
    }
  }

  const onDeletePageHandler = () => {
    formsStore.deletePage(formsStore.currentPageForm - 1)
  }

  const SortableList = SortableContainer(({ children }) => <div>{children}</div>)

  const SortableItem = SortableElement(({ value, index, elemIndex }) =>
    <Elements key={index} elemIndex={elemIndex} type={value}/>)

  const onSortEnd = ({ oldIndex, newIndex }) => {
    let newArrPos = arrayMove(elementListOnPage, oldIndex, newIndex)
    formsStore.setElementPosition(newArrPos)
  }

  return (
    <Wrapper>
      <EditorHeading theme={commonStore.appTheme}>
        <h1>{buyersStore.formCreateValues.name}</h1>
        <p>{buyersStore.formCreateValues.description}</p>
      </EditorHeading>
      <EditorMain onDragOver={e => e.preventDefault()}
                  onDrop={() => onDropItemHandler(droppedItem)}>
        {
          elementListOnPage.length === 0
            ? <EmptyWrapper>This page is empty. Drag fields from the left pannel and drop here!</EmptyWrapper>
            : <SortableList
              pressDelay={200} helperClass={'dragging-element'}
              onSortEnd={onSortEnd}>
              {
                elementListOnPage.map((elem, index) =>
                  <SortableItem key={index} index={index} elemIndex={index} value={elem}/>)
              }
            </SortableList>
        }
      </EditorMain>
      <PageFooter>
        {
          formsStore.pageCount === 1
            ? <p style={{ marginBottom: 0 }}>&nbsp;</p>
            : <Popconfirm
              icon={<Icon type="question-circle-o" style={{ color: 'red' }}/>}
              onConfirm={onDeletePageHandler}
              okType={'danger'} okText={'Delete'}
              title={'Delete this page?'}>
              <a href="" style={{ color: 'rgb(244, 67, 54)' }}>Delete page</a>
            </Popconfirm>
        }
        <PageIndicator>
          {
            formsStore.currentPageForm === 1
              ? null
              : <FormPageNavigation
                onClick={() => formsStore.pageNavigation('back')}
                type={'back'}
                color={commonStore.appTheme}>
                Back
              </FormPageNavigation>
          }
          Page {formsStore.currentPageForm} / {formsStore.pageCount}
          {
            formsStore.currentPageForm === formsStore.pageCount
              ? null
              : <FormPageNavigation
                onClick={() => formsStore.pageNavigation('next')}
                type={'next'}
                color={commonStore.appTheme}>
                Next
              </FormPageNavigation>
          }
        </PageIndicator>
      </PageFooter>
    </Wrapper>
  )
}

export default withRouter(inject('buyersStore', 'commonStore', 'formsStore')(observer(Editor)))