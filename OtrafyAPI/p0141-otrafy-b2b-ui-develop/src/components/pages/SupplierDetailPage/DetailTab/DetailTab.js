import React, { useEffect } from 'react'
import RequestListTable from '../RequestListTable'
import { LeftColumnContent, RightColumnContent } from '../CustomStyled'
import { DetailWrapper } from './DetailTabStyled'
import { inject, observer } from 'mobx-react'
import SupplierDetailCard from '../SupplierDetailCard'
import SupplierDetailEditCard from '../SupplierDetailEditCard'

const DetailTab = ({ suppliersStore, tagsStore }) => {

  useEffect(() => {
    suppliersStore.toggleEditMode(false)
  }, [])

  useEffect(() => {
    tagsStore.getAllTags('')
  }, [])

  return (
    <DetailWrapper>
      <LeftColumnContent>
        {
          suppliersStore.editMode
            ? <SupplierDetailEditCard tagsList={tagsStore.tagsList}/>
            : <SupplierDetailCard/>
        }
      </LeftColumnContent>
      <RightColumnContent>
        <RequestListTable/>
      </RightColumnContent>
    </DetailWrapper>
  )
}

export default inject('suppliersStore', 'tagsStore')(observer(DetailTab))