import React, { useEffect, useState } from 'react'
import { inject, observer } from 'mobx-react'
import { withRouter } from 'react-router-dom'
import moment from 'moment'
import {
  CommentsBlockWrapper, CommentBlockHeading,
  Comment, TimelineWrapper, CommentZone,
} from './CustomStyled'
import { Timeline, Input, Dropdown, Menu } from 'antd'
// Icons
import multilineIcon from '../../../assets/form-elems/multiLine@2x.png'
import newestIcon from '../../../assets/icons/newest-sort-icon@2x.png'
import oldestIcon from '../../../assets/icons/oldest-sort-icon@2x.png'

const { TextArea } = Input

const CommentsBlock = ({ match, formsRequestsStore }) => {

  const formRequestId = match.params.formRequestId
  const commentMsg = 'Input your comment here. \nPress Enter to add a comment.'
  const [sortType, setSortType] = useState(1)
  const [addCommentData, setAddCommentData] = useState('')

  const handleTypeComment = e => {
    setAddCommentData(e.target.value)
  }

  const handleAddComment = e => {
    if (e.keyCode === 13) {
      let commentMsg = {
        comments: addCommentData,
      }
      formsRequestsStore.addComment(formRequestId, commentMsg, sortType)
    }
  }

  const menu = (
    <Menu>
      <Menu.Item onClick={() => setSortType(1)}>
        Newest
      </Menu.Item>
      <Menu.Item onClick={() => setSortType(0)}>
        Oldest
      </Menu.Item>
    </Menu>
  )

  useEffect(() => {
    setAddCommentData(null)
  }, [formsRequestsStore.formRequestComments])

  useEffect(() => {
    formsRequestsStore.getRequestComments(`?formrequestId=${formRequestId}&SortType=${sortType}`)
  }, [sortType])

  return (
    <CommentsBlockWrapper>
      <CommentBlockHeading>
        <h2>Comments</h2>
        <div className={'action'}>
          Sort by:
          <Dropdown overlay={menu} trigger={['click']} placement={'bottomRight'}>
            <div className="dropdown-trigger">
              {
                sortType === 1
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
      </CommentBlockHeading>
      <TimelineWrapper>
        {
          formsRequestsStore.formRequestComments.map(comment =>
            <Timeline.Item key={comment.id}>
              <Comment>
                <div className="heading">
                  <div className="user">
                    by {comment.createdBy}
                  </div>
                  <time className="datetime">
                    {moment(comment.dateUpdated).format('DD/MM/YYYY')}
                  </time>
                </div>
                <div className="content">
                  {comment.message}
                </div>
              </Comment>
            </Timeline.Item>,
          )
        }
      </TimelineWrapper>
      <CommentZone icon={multilineIcon}>
        <TextArea
          value={addCommentData}
          onKeyDown={e => handleAddComment(e)}
          onChange={e => handleTypeComment(e)}
          rows={4}
          placeholder={commentMsg}/>
      </CommentZone>
    </CommentsBlockWrapper>
  )
}

export default withRouter(inject('formsRequestsStore')(observer(CommentsBlock)))