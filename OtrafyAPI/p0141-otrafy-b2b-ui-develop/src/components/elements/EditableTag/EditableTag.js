import React, { useState } from 'react'
import PropTypes from 'prop-types'
import { Tooltip, Popconfirm, Icon } from 'antd'
import {
  Wrapper, Tag, ShowAllTagsButton,
} from './CustomStyled'

const EditableTag = ({ tags, id, onRemove }) => {

  const [showAllTags, setShowAllTags] = useState(false)
  const warningTxt = `Are you sure you want to delete this tag?`

  const TagContent = () => {
    if (tags.length === 0) {
      return null
    }
    return (
      <React.Fragment>
        {
          showAllTags
            ? tags.map(tag =>
              <Tag key={tag}>
                {tag}
                <Popconfirm title={warningTxt}
                            icon={<Icon type="question-circle-o" style={{ color: 'red' }} />}
                            okType={'danger'}
                            okText="Delete" cancelText="Cancel"
                            onConfirm={() => onRemove(id, tag)}>
                  <span>x</span>
                </Popconfirm>
              </Tag>)
            : tags.slice(0, 2).map(tag =>
              <Tag key={tag}>
                {tag}
                <Popconfirm title={warningTxt}
                            icon={<Icon type="question-circle-o" style={{ color: 'red' }} />}
                            okType={'danger'}
                            okText="Delete" cancelText="Cancel"
                            onConfirm={() => onRemove(id, tag)}>
                  <span>x</span>
                </Popconfirm>
              </Tag>)
        }
        {
          showAllTags
            ? null
            : tags.length <= 2
            ? null
            : <Tooltip title={'Show all tags'}>
              <ShowAllTagsButton onClick={() =>
                setShowAllTags(true)}>...</ShowAllTagsButton>
            </Tooltip>
        }
      </React.Fragment>
    )
  }

  return (
    <Wrapper>
      <TagContent/>
    </Wrapper>
  )
}

EditableTag.propTypes = {
  tags: PropTypes.array.isRequired,
  id: PropTypes.string.isRequired,
}

export default EditableTag