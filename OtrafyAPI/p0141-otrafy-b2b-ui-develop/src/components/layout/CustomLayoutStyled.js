import React from 'react'
import styled from 'styled-components'
import bg from '../../assets/imgs/auth-bg@2x.png'

// Auth layout
export const AuthLayoutWrapper = styled.div`
  min-height: 100vh;
  position: relative;
  padding: 30px 0;
  &:after {
    display: block;
    content: '';
    width: 100vw;
    height: 30.9vw;
    position: absolute;
    left: 0;
    bottom: 0;
    background: url(${bg}) no-repeat center center;
    background-size: 100% 100%;
    pointer-events: none;
  }
`
export const LogoWrapper = styled.div`
  display: block;
  width: 128px;
  height: 93px;
  margin: 0 auto 30px;
  img {
    width: 100%;
    height: auto;
  }
`
export const Form = styled.div`
  display: block;
  margin-left: auto;
  margin-right: auto;
  max-width: 640px;
  position: relative;
  z-index: 1;
  background-color: #fff;
  padding: 40px 70px 25px;
  border: 1px solid #e6e6e6;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.3);
  border-radius: 10px;
  margin-bottom: 25px;
  @media screen and (max-width: 768px) {
    width: calc(100% - 30px);
    padding: 40px 20px 20px;
  }
`
export const AuthFooter = styled.div`
  display: block;
  margin-left: auto;
  margin-right: auto;
  text-align: center;
  position: relative;
  z-index: 1;
  p, a {
    color: white;
    font-size: 12px;
  }
  ul {
    display: flex;
    justify-content: center;
    text-align: center;
    margin-bottom: 0;
    li {
      display: flex;
      align-items: center;
      line-height: 1;
      &:after {
        display: block;
        content: '';
        margin: 0 20px;
        width: 1px;
        height: 100%;
        background-color: #fff;
      }
      &:last-child {
        &:after {
          display: none;
        }
      }
      a {
        text-decoration: underline;
        &:hover {
          color: white;
        }
      }
    }
  }
`
// Main layout
export const NormalLayoutWrapper = styled.div`
  padding: 15px;
  background-color: #F2F3F9;
  display: flex;
  justify-content: flex-end;
  flex-wrap: wrap;
  align-items: stretch;
  min-height: 100vh;
  position: relative;
  @media screen and (max-width: 1024px) {
    margin-top: 82px;
  }
`
export const SidebarWrapper = styled.aside`
  transition: ease .2s;
  position: fixed;
  top: 15px;
  left: 15px;
  height: calc(100% - 30px);
`
export const MainContentWrapper = styled.div`
  padding-left: 20px;
  transition: ease .2s;
  @media screen and (max-width: 1024px) {
    width: 100% !important;
    padding-left: 0;
  }
`