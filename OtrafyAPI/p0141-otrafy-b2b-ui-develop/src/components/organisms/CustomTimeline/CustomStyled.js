import React from 'react'
import { Timeline } from 'antd'
import styled from 'styled-components'

export const TimelineWrapper = styled(Timeline)``
export const TimelineItem = styled(Timeline.Item)`
  .ant-timeline-item-content {
    display: flex;
    justify-content: space-between;
    flex-wrap: wrap;  
    align-items: flex-end;
    .info {
      h3 {
        color: #666666;
        font-size: 14px;
        margin-bottom: 2px;
      }
      p {
        font-size: 12px;
        color: #919699;
        margin-bottom: 0;
      }
    }
    .date {
      color: #919699;
      font-size: 12px;
    }
  }
`