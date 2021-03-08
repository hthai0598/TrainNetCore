import React, { useState, Fragment } from 'react'
import { Button } from 'antd'
import SignatureCanvas from 'react-signature-canvas'
import {
  SignatureEditButtonRow, SignatureWrapper, ButtonGroup
} from './CustomSignaturePadStyled'

const CustomSignaturePad = ({ onSave, data }) => {

  const [editMode, setEditMode] = useState(false)
  const [tempSignature, setTempSignature] = useState('')

  let sigCanvasRef = {}

  const handleSaveSignature = () => {
    onSave(sigCanvasRef.toDataURL())
    setTempSignature(sigCanvasRef.toDataURL())
    setEditMode(false)
  }

  return (
    <Fragment>
      {
        editMode
          ? <SignatureCanvas
            ref={ref => {
              sigCanvasRef = ref
            }}
            penColor='green' throttle={1}
            canvasProps={{ id: 'sigCanvas' }}/>
          : <SignatureWrapper>
            {tempSignature ? <img src={tempSignature} alt="signature"/> : data}
          </SignatureWrapper>
      }
      {
        editMode
          ? <SignatureEditButtonRow>
            <Button onClick={() => sigCanvasRef.clear()}>
              Clear
            </Button>
            <ButtonGroup>
              <Button
                type={'danger'} ghost
                onClick={() => setEditMode(false)}>
                Cancel
              </Button>
              <Button
                onClick={handleSaveSignature}
                type={'primary'}>
                Save signature
              </Button>
            </ButtonGroup>
          </SignatureEditButtonRow>
          : <Button
            onClick={() => setEditMode(true)}
            type={'primary'}>
            Edit signature
          </Button>
      }
    </Fragment>
  )
}

export default CustomSignaturePad