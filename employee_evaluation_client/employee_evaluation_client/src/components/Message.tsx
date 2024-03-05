import React, { useEffect, useState } from 'react';
import { Alert } from 'antd';

const Message = ({ message, type, duration }) => {
  const [visible, setVisible] = useState(true);
  const [key, setKey] = useState(0);

  useEffect(() => {
    const timeout = setTimeout(() => {
      setVisible(false);
    }, duration);

    return () => clearTimeout(timeout);
  }, [duration]);

  useEffect(() => {
    setKey(prevKey => prevKey + 1);
    setVisible(true);
  }, [message, type]);

  return (
    <>
      {visible && (
        <div style={{
          position: 'fixed',
          top: '20%',
          left: '37%',
          width: '50%',
          maxWidth: '400px',
          zIndex: 1000
        }}>
          <Alert style={{direction:'rtl'}}
            key={key}
            message={message}
            type={type}
            showIcon
            closable
            onClose={() => setVisible(false)} 
          />
        </div>
      )}
    </>
  );
};

export default Message;
