import React  from 'react'
import PropTypes from 'prop-types'
import {
  Wrapper, Tag, ShowAllTagsButton,
} from './CustomStyled'
import { inject, observer } from 'mobx-react'
import { toJS } from 'mobx'
import commonStore from '../../../stores/commonStore'

const NormalTag = ({ tags, commonStore }) => {

  return (
    <Wrapper>
      {
        tags.slice(0, 2).map(tag =>
          <Tag key={tag}>
            {tag}
          </Tag>)
      }
      {
        tags.length <= 2
          ? null
          : <ShowAllTagsButton onMouseMove={e => commonStore.setMouseCordinate(e)}>...</ShowAllTagsButton>
      }
      <div className='tag-wrapper' style={{
        left: toJS(commonStore.mouseCordinate.x),
        top: toJS(commonStore.mouseCordinate.y),
      }}>
        {
          tags.map(tag =>
            <Tag key={tag}>
              {tag}
            </Tag>)
        }
      </div>
    </Wrapper>
  )

}

NormalTag.propTypes = {
  tags: PropTypes.array,
}

export default inject('commonStore')(observer(NormalTag))