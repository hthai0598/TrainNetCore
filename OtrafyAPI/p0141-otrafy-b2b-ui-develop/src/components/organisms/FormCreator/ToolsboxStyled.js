import React from 'react'
import styled from 'styled-components'

export const Wrapper = styled.ul`
  background: #FFFFFF;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  border-radius: 10px;
  padding: 15px;
  width: 33.5%;
`
export const List = styled.ul`
  display: flex;
  flex-wrap: wrap;
  li {
    border-radius: 4px;
    border: 1px solid #E3E5E5;
    margin-bottom: 3px;
    padding: 5px 10px 5px 35px;
    position: relative;
    @media screen and (min-width: 1367px) {
      width: 49%;
      margin-right: 1%;
    }
    @media screen and (max-width: 1366px) {
      width: 100%;    
    }
    &:hover {
      cursor: pointer;
    }
    .img {
      width: 35px;
      position: absolute;
      left: 0;
      top: 50%;
      text-align: center;
      transform: translateY(-50%);
      display: flex;
      justify-content: center;
      align-items: center;
    }
  }
`
export const FieldsTitle = styled.div`
  font-size: 14px;
  line-height: 1.6;
  font-weight: 500;
  color: #000;
  margin-bottom: 15px;
`