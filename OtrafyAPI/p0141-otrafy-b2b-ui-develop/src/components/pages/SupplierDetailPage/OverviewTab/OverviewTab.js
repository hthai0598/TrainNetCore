import React, { useEffect, useState } from 'react'
import { inject, observer } from 'mobx-react'
import { withRouter } from 'react-router-dom'
import { OverviewWrapper, TimelineWrapper, TimelineHeading } from './OverviewTabStyled'
import { LeftColumnContent, RightColumnContent } from '../CustomStyled'
import { Menu, Dropdown } from 'antd'
import CustomTimeline from '../../../organisms/CustomTimeline'
import SupplierDetailCard from '../SupplierDetailCard'
import SupplierDetailEditCard from '../SupplierDetailEditCard'
import RequestListTable from '../RequestListTable'
// Icons
import newestIcon from '../../../../assets/icons/newest-sort-icon@2x.png'
import oldestIcon from '../../../../assets/icons/oldest-sort-icon@2x.png'

const OverviewTab = ({ formsRequestsStore, suppliersStore, tagsStore }) => {

  const [sortTimelineByNewest, setSortTimelineByNewest] = useState(true)

  const timelineData = [
    {
      formName: 'Form 1',
      status: 'sent',
      buyerName: 'Buyer 1',
      date: '09/23/2019',
    },
    {
      formName: 'Form 2',
      status: 'completed',
      buyerName: 'Buyer 1',
      date: '09/23/2019',
    },
    {
      formName: 'Form 3',
      status: 'completed',
      buyerName: 'Buyer 1',
      date: '09/23/2019',
    },
    {
      formName: 'Form 2',
      status: 'sent',
      buyerName: 'Buyer 1',
      date: '09/23/2019',
    },
    {
      formName: 'Form 3',
      status: 'reminded',
      buyerName: 'Buyer 1',
      date: '09/23/2019',
    },
  ]

  const menu = (
    <Menu>
      <Menu.Item onClick={() => setSortTimelineByNewest(true)}>
        Newest
      </Menu.Item>
      <Menu.Item onClick={() => setSortTimelineByNewest(false)}>
        Oldest
      </Menu.Item>
    </Menu>
  )

  useEffect(() => {
    suppliersStore.toggleEditMode(false)
  }, [])

  useEffect(() => {
    tagsStore.getAllTags('')
  }, [])

  return (
    <OverviewWrapper>
      <LeftColumnContent>
        {
          suppliersStore.editMode
            ? <SupplierDetailEditCard tagsList={tagsStore.tagsList}/>
            : <SupplierDetailCard/>
        }
        <TimelineWrapper>
          <TimelineHeading>
            <div className="title">Timeline</div>
            <div className="action">
              Sort by:
              <Dropdown overlay={menu} placement="bottomLeft">
                <div className={'dropdown-trigger'}>
                  {
                    sortTimelineByNewest
                      ? <p>
                        Newest <img src={newestIcon} alt=""/>
                      </p>
                      : <p>
                        Oldest <img src={oldestIcon} alt=""/>
                      </p>
                  }
                </div>
              </Dropdown>
            </div>
          </TimelineHeading>
          <CustomTimeline data={timelineData}/>
        </TimelineWrapper>
      </LeftColumnContent>
      <RightColumnContent>
        <RequestListTable/>
      </RightColumnContent>
    </OverviewWrapper>
  )
}

export default withRouter(inject('formsRequestsStore', 'suppliersStore', 'tagsStore')(observer(OverviewTab)))