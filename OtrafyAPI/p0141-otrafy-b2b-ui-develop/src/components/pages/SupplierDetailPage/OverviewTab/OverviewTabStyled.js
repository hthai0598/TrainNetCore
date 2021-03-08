import React from 'react'
import styled from 'styled-components'

export const OverviewWrapper = styled.section`
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  flex-wrap: wrap;
`
export const TimelineWrapper = styled.div`
  background: #FFFFFF;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  border-radius: 10px;
  padding: 15px;
`
export const TimelineHeading = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 15px;
  .title {
    color: #000;
    font-size: 14px;
    font-weight: 500;
  }
  .action {
    display: flex;
    justify-content: space-between;
    align-items: center;
    .dropdown-trigger {
      background: #FFFFFF;
      border: 1px solid #E3E5E5;
      box-sizing: border-box;
      border-radius: 4px;
      height: 32px;
      display: flex;
      align-items: center;
      justify-content: center;
      text-align: center;
      padding: 0 10px;
      margin-left: 10px;
      &:hover {
        cursor: pointer;
      }
      p {
        margin-bottom: 0;
        img {
          margin-left: 12px;
          width: 12px;
        }
      }
    }
  }
`