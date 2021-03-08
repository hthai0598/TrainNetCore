import React from 'react'
import styled from 'styled-components'
import { Timeline } from 'antd'

export const CommentsBlockWrapper = styled.div`
  display: block;
  width: 325px;
  background: #FFFFFF;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  border-radius: 10px;
  padding: 0 15px 15px;
`
export const CommentBlockHeading = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 10px 0;
  h2 {
    color: #000;
    font-size: 14px;
    font-weight: 500;
    margin-bottom: 0;
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
export const TimelineWrapper = styled(Timeline)`
  padding: 10px 0 !important;
`
export const Comment = styled.div`
  .heading {
    display: flex;
    justify-content: space-between;
    align-items: center;
    font-size: 12px;
    color: #919699;
  }
  .content {
    color: #020202;
    font-size: 14px;
  }
`
export const CommentZone = styled.div`
  position: relative;
  &:after {
    display: block;
    content: '';
    width: 11px;
    height: 7px;
    background: url(${props => props.icon}) no-repeat center center;
    background-size: 100% 100%;
    position: absolute;
    right: 8px;
    bottom: 8px;
    opacity: .6;
    pointer-events: none;
  }
  textarea {
    resize: none;
  }
`