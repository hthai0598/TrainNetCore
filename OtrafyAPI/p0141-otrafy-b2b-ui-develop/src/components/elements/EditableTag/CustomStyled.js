import React from 'react'
import styled from 'styled-components'

export const Wrapper = styled.ul`
  position: relative;  
  display: flex;
  flex-wrap: wrap;
  align-items: flex-start;
  margin-bottom: -5px;
  max-width: 300px;
`
export const Tag = styled.li`
  color: #666;
  font-size: 14px;
  background-color: #F5F6F8;
  border-radius: 100px;
  padding: 2px 30px 2px 10px;
  position: relative;
  margin-right: 5px;
  margin-bottom: 5px;
  white-space: nowrap;
  span {
    width: 14px;
    height: 14px;
    line-height: 13px;
    color: white;
    background-color: #e5e5e5;
    border-radius: 50%;
    position: absolute;
    right: 6px;
    top: 50%;
    transform: translateY(-50%);
    text-align: center;
    font-size: 10px;
    &:hover {
      cursor: pointer;
    }
  }
`
export const ShowAllTagsButton = styled.span`
  color: #666666;
  font-size: 14px;
  background-color: #F5F6F8;
  border-radius: 50%;
  text-align: center;
  width: 25px;
  height: 25px;
  line-height: 26px;
  &:hover {
    cursor: pointer;
  }
`